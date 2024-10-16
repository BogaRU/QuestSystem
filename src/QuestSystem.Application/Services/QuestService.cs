using QuestSystem.Application.DTOs;
using QuestSystem.Application.Interfaces;
using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestSystem.Application.Services
{
    public class QuestService : IQuestService
    {
        private readonly IQuestRepository _questRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerQuestRepository _playerQuestRepository;
        private readonly IPlayerItemRepository _playerItemRepository;

        public QuestService(IQuestRepository questRepository, IPlayerRepository playerRepository, IPlayerQuestRepository playerQuestRepository, IPlayerItemRepository playerItemRepository)
        {
            _questRepository = questRepository;
            _playerRepository = playerRepository;
            _playerQuestRepository = playerQuestRepository;
            _playerItemRepository = playerItemRepository;
        }

        public async Task<IEnumerable<Quest>> GetAvailableQuestsAsync(Guid playerId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                throw new ArgumentException("Игрок не найден.");

            var allQuests = await _questRepository.GetAllAsync();
            var completedQuests = await _playerQuestRepository.GetCompletedQuestsAsync(playerId);

            var availableQuests = new List<Quest>();

            foreach (var quest in allQuests)
            {
                if (quest.MinimumLevel > player.Level)
                    continue;

                bool prerequisitesMet = quest.RequiredCompletedQuests.All(qId => completedQuests.Contains(qId));

                if (!prerequisitesMet)
                    continue;

                bool alreadyAcceptedOrCompleted = await _playerQuestRepository.IsQuestAcceptedOrCompletedAsync(playerId, quest.Id);
                if (alreadyAcceptedOrCompleted)
                    continue;

                availableQuests.Add(quest);
            }

            return availableQuests;
        }

        public async Task<PlayerQuest> AcceptQuestAsync(Guid playerId, Guid questId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                throw new ArgumentException("Игрок не найден.");

            var quest = await _questRepository.GetByIdAsync(questId);
            if (quest == null)
                throw new ArgumentException("Квест не найден.");

            if (quest.MinimumLevel > player.Level)
                throw new InvalidOperationException("Уровень игрока недостаточен для принятия этого квеста.");

            var completedQuests = await _playerQuestRepository.GetCompletedQuestsAsync(playerId);
            bool prerequisitesMet = quest.RequiredCompletedQuests.All(qId => completedQuests.Contains(qId));
            if (!prerequisitesMet)
                throw new InvalidOperationException("Не выполнены все необходимые предварительные квесты.");

            var activeQuestsCount = (await _playerQuestRepository.GetByPlayerIdAsync(playerId))
                                    .Count(pq => pq.Status == QuestStatus.Accepted || pq.Status == QuestStatus.InProgress);

            if (activeQuestsCount >= 10)
                throw new InvalidOperationException("Превышено максимальное количество активных квестов.");

            var playerQuest = new PlayerQuest(playerId, questId);
            await _playerQuestRepository.AddAsync(playerQuest);
            await _playerQuestRepository.SaveChangesAsync();

            return playerQuest;
        }

        public async Task<PlayerQuest> UpdateQuestProgressAsync(Guid playerQuestId, QuestProgressUpdateDto progressUpdate)
        {
            var playerQuest = await _playerQuestRepository.GetByIdAsync(playerQuestId);
            if (playerQuest == null)
                throw new ArgumentException("Квест игрока не найден.");

            if (playerQuest.Status == QuestStatus.Completed || playerQuest.Status == QuestStatus.Finished)
                return playerQuest;

            if (progressUpdate.CollectedItemIds.Any())
            {
                var condition = playerQuest.Quest.Conditions.FirstOrDefault(c => c.Type == QuestConditionType.CollectItems);
                if (condition != null)
                {
                    var playerCollectedItemIds = progressUpdate.CollectedItemIds;

                    var requiredItemIds = condition.RequiredItemIds;

                    await playerQuest.Progress.UpdateCollectedItems(playerCollectedItemIds.ToList(), requiredItemIds.ToList(), condition.RequiredAmount);
                }
            }

            if (progressUpdate.DefeatedMonsters.HasValue)
            {
                var condition = playerQuest.Quest.Conditions.FirstOrDefault(c => c.Type == QuestConditionType.DefeatMonsters);
                if (condition != null)
                {
                    await playerQuest.Progress.UpdateDefeatedMonsters(progressUpdate.DefeatedMonsters.Value, condition.RequiredAmount);
                }
            }

            if (progressUpdate.VisitedLocations.HasValue)
            {
                var condition = playerQuest.Quest.Conditions.FirstOrDefault(c => c.Type == QuestConditionType.VisitLocations);
                if (condition != null)
                {
                    await playerQuest.Progress.UpdateVisitedLocations(progressUpdate.VisitedLocations.Value, condition.RequiredAmount);
                }
            }

            if (playerQuest.Status == QuestStatus.Accepted && !playerQuest.Progress.IsCompleted)
            {
                await playerQuest.Start();
            }

            if (playerQuest.Progress.IsCompleted)
            {
                await playerQuest.Complete();
            }

            await _playerQuestRepository.Update(playerQuest);
            await _playerQuestRepository.SaveChangesAsync();

            return playerQuest;
        }

        public async Task FinishQuestAsync(Guid playerQuestId)
        {
            var playerQuest = await _playerQuestRepository.GetByIdAsync(playerQuestId);
            if (playerQuest == null)
                throw new ArgumentException("Квест игрока не найден.");

            if (playerQuest.Status == QuestStatus.Finished)
                throw new InvalidOperationException("Квест уже завершен.");

            if (playerQuest.Status != QuestStatus.Completed)
            {
                var quest = playerQuest.Quest;
                var incompleteConditions = new List<string>();

                foreach (var condition in quest.Conditions)
                {
                    switch (condition.Type)
                    {
                        case QuestConditionType.CollectItems:
                            if (playerQuest.Progress.CollectedItems < condition.RequiredAmount)
                                incompleteConditions.Add($"Собрать {condition.RequiredAmount} предметов.");
                            break;
                        case QuestConditionType.DefeatMonsters:
                            if (playerQuest.Progress.DefeatedMonsters < condition.RequiredAmount)
                                incompleteConditions.Add($"Победить {condition.RequiredAmount} монстров.");
                            break;
                        case QuestConditionType.VisitLocations:
                            if (playerQuest.Progress.VisitedLocations < condition.RequiredAmount)
                                incompleteConditions.Add($"Посетить {condition.RequiredAmount} локаций.");
                            break;
                    }
                }

                if (incompleteConditions.Any())
                {
                    var message = "Квест не может быть завершен. Не выполнены условия:\n" + string.Join("\n", incompleteConditions);
                    throw new InvalidOperationException(message);
                }
            }

            foreach (var reward in playerQuest.Quest.Rewards)
            {
                switch (reward.Type)
                {
                    case RewardType.Item:
                        if (reward.ItemIds.Any())
                        {
                            foreach (var itemId in reward.ItemIds)
                            {
                                var playerItem = await _playerItemRepository.GetPlayerItemAsync(playerQuest.PlayerId, itemId);
                                if (playerItem != null)
                                {
                                    await playerItem.AddQuantity(1); // Предположим, что награда — 1 предмет
                                    await _playerItemRepository.Update(playerItem);
                                }
                                else
                                {
                                    var newPlayerItem = new PlayerItem(playerQuest.PlayerId, itemId, 1);
                                    await _playerItemRepository.AddAsync(newPlayerItem);
                                }
                            }
                        }    
                        break;
                    case RewardType.Experience:
                        await playerQuest.Player.AddExperience(reward.Amount);
                        await _playerRepository.Update(playerQuest.Player);
                        break;
                    case RewardType.Money:
                        await playerQuest.Player.AddMoney(reward.Amount);
                        await _playerRepository.Update(playerQuest.Player);
                        break;
                }
            }

            await playerQuest.Finish();
            await _playerQuestRepository.Update(playerQuest);
            await _playerQuestRepository.SaveChangesAsync();
        }
    }
}
