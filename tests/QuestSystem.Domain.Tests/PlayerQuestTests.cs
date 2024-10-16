using NUnit.Framework;
using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace QuestSystem.Domain.Tests
{
    [TestFixture]
    public class PlayerQuestTests
    {
        private Guid _playerId;
        private Guid _questId;
        private PlayerQuest _playerQuest;

        [SetUp]
        public void Setup()
        {
            _playerId = Guid.NewGuid();
            _questId = Guid.NewGuid();
            _playerQuest = new PlayerQuest(_playerId, _questId);
        }

        [Test]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            Assert.That(_playerQuest.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(_playerQuest.PlayerId, Is.EqualTo(_playerId));
            Assert.That(_playerQuest.QuestId, Is.EqualTo(_questId));
            Assert.That(_playerQuest.Status, Is.EqualTo(QuestStatus.Accepted));
            Assert.IsNotNull(_playerQuest.Progress);
        }

        [Test]
        public async Task Start_ShouldChangeStatusToInProgress_WhenStatusIsAccepted()
        {
            await _playerQuest.Start();

            Assert.That(_playerQuest.Status, Is.EqualTo(QuestStatus.InProgress));
        }

        [Test]
        public async Task Start_ShouldNotChangeStatus_WhenStatusIsNotAccepted()
        {
            _playerQuest = new PlayerQuest(_playerId, _questId);
            await _playerQuest.Start();

            await _playerQuest.Start();

            Assert.That(_playerQuest.Status, Is.EqualTo(QuestStatus.InProgress));
        }

        [Test]
        public async Task Complete_ShouldNotChangeStatusToCompleted_WhenProgressIsNotCompleted()
        {

            _playerQuest = new PlayerQuest(_playerId, _questId);
            await _playerQuest.Start();
            await _playerQuest.Complete();

            Assert.That(_playerQuest.Progress.IsCompleted, Is.False);
            Assert.That(_playerQuest.Status, Is.EqualTo(QuestStatus.InProgress));
        }

        [Test]
        public async Task UpdateProgress_ShouldUpdateProgressAndChangeStatus()
        {
            _playerQuest = new PlayerQuest(_playerId, _questId);
            await _playerQuest.Start();
            var progress = _playerQuest.Progress;
            var itemGuid = Guid.NewGuid();

            await progress.UpdateCollectedItems(new List<Guid> { itemGuid }, new List<Guid> { itemGuid }, 1);
            Assert.That(_playerQuest.Progress.IsEnoughItems, Is.True);

            await progress.UpdateDefeatedMonsters(1, 1);
            Assert.That(_playerQuest.Progress.IsEnoughMonsters, Is.True);

            await progress.UpdateVisitedLocations(1, 1);
            Assert.That(_playerQuest.Progress.IsEnoughLocations, Is.True);

            Assert.That(_playerQuest.Progress.IsCompleted, Is.True);

            await _playerQuest.CheckProgress();
            Assert.That(_playerQuest.Status, Is.EqualTo(QuestStatus.Completed));
        }
    }
}
