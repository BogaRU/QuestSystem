using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Enums;
using System;

namespace QuestSystem.Domain.ValueObjects
{
    public class QuestProgress
    {
        public int CollectedItems { get; private set; }
        public int DefeatedMonsters { get; private set; }
        public int VisitedLocations { get; private set; }
        public bool IsEnoughItems { get; private set; }
        public bool IsEnoughMonsters { get; private set; }
        public bool IsEnoughLocations { get; private set; }

        public bool IsCompleted { get; private set; }

        public QuestProgress()
        {
            CollectedItems = 0;
            DefeatedMonsters = 0;
            VisitedLocations = 0;
            IsCompleted = false;
            IsEnoughLocations = false;
            IsEnoughMonsters = false;
            IsEnoughItems = false;
            
        }

        public async Task UpdateCollectedItems(List<Guid> collectedItemIds, List<Guid> requiredItemIds, int requiredAmount)
        {
            var matchedItems = collectedItemIds.Intersect(requiredItemIds).ToList();

            int collectedItemCount = matchedItems.Count; // Для простоты сравниваем по количеству совпадающих предметов

            if (collectedItemCount >= requiredAmount)
                IsEnoughItems = true;

            CollectedItems = Math.Min(collectedItemCount, requiredAmount);

            CheckCompletion();
        }

        public async Task UpdateDefeatedMonsters(int amount, int requiredAmount)
        {
            if (amount < DefeatedMonsters)
                throw new ArgumentException("Прогресс не может уменьшаться.");
            else if (amount >= requiredAmount)
                IsEnoughMonsters = true;
            DefeatedMonsters = Math.Min(amount, requiredAmount);
            CheckCompletion();
        }

        public async Task UpdateVisitedLocations(int amount, int requiredAmount)
        {
            if (amount < VisitedLocations)
                throw new ArgumentException("Прогресс не может уменьшаться.");
            else if (amount >= requiredAmount)
                IsEnoughLocations = true;
            VisitedLocations = Math.Min(amount, requiredAmount);
            CheckCompletion();
        }

        private void CheckCompletion()
        {
            IsCompleted = IsEnoughItems && IsEnoughLocations && IsEnoughMonsters;
        }
    }
}
