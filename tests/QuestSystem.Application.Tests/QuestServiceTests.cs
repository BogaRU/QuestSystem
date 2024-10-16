using NUnit.Framework;
using Moq;
using QuestSystem.Application.DTOs;
using QuestSystem.Application.Interfaces;
using QuestSystem.Application.Services;
using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestSystem.Domain.Interfaces;

namespace QuestSystem.Application.Tests
{
    [TestFixture]
    public class QuestServiceTests
    {
        private Mock<IQuestRepository> _questRepoMock;
        private Mock<IPlayerRepository> _playerRepoMock;
        private Mock<IPlayerQuestRepository> _playerQuestRepoMock;
        private Mock<IPlayerItemRepository> _playerItemRepoMock;
        private IQuestService _questService;

        [SetUp]
        public void Setup()
        {
            _questRepoMock = new Mock<IQuestRepository>();
            _playerRepoMock = new Mock<IPlayerRepository>();
            _playerQuestRepoMock = new Mock<IPlayerQuestRepository>();
            _playerItemRepoMock = new Mock<IPlayerItemRepository>();
            _questService = new QuestService(_questRepoMock.Object, _playerRepoMock.Object, _playerQuestRepoMock.Object, _playerItemRepoMock.Object);
        }

        private async Task<Quest> CreateQuest(string title, string description, int minimumLevel, List<QuestCondition> conditions, List<QuestReward> rewards, List<Guid> requiredCompletedQuests = null)
        {
            var quest = new Quest(title, description, minimumLevel);

            foreach (var condition in conditions)
            {
                await quest.AddCondition(condition);
            }

            foreach (var reward in rewards)
            {
                await quest.AddReward(reward);
            }

            if (requiredCompletedQuests != null)
            {
                foreach (var qId in requiredCompletedQuests)
                {
                    await quest.AddRequiredCompletedQuest(qId);
                }
            }

            return quest;
        }

        [Test]
        public void AcceptQuestAsync_ShouldThrowException_WhenPlayerNotFound()
        {
            var playerId = Guid.NewGuid();
            var questId = Guid.NewGuid();
            _playerRepoMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync((Player)null);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _questService.AcceptQuestAsync(playerId, questId));
            Assert.That(ex.Message, Is.EqualTo("Игрок не найден."));
        }

        [Test]
        public async Task AcceptQuestAsync_ShouldThrowException_WhenQuestNotFound()
        {
            var playerId = Guid.NewGuid();
            var questId = Guid.NewGuid();
            var player = new Player("TestPlayer", 10);
            _playerRepoMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync(player);
            _questRepoMock.Setup(repo => repo.GetByIdAsync(questId)).ReturnsAsync((Quest)null);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _questService.AcceptQuestAsync(playerId, questId));
            Assert.That(ex.Message, Is.EqualTo("Квест не найден."));
        }
    }
}
