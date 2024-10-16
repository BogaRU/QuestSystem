using NUnit.Framework;
using QuestSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestSystem.Domain.Tests
{
    [TestFixture]
    public class PlayerTests
    {
        private Player _player;

        [SetUp]
        public void Setup()
        {
            _player = new Player("TestPlayer", 1, 0, 0);
        }

        [Test]
        public async Task UpdateLevel_ShouldUpdateLevel_WhenNewLevelIsHigher()
        {
            await _player.UpdateLevel(2);
            Assert.That(_player.Level, Is.EqualTo(2));
        }

        [Test]
        public void UpdateLevel_ShouldThrowException_WhenNewLevelIsLower()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _player.UpdateLevel(0));
        }

        [Test]
        public async Task AddExperience_ShouldIncreaseExperienceAndLevel_WhenThresholdIsReached()
        {
            await _player.AddExperience(150); // 1 уровень до 2-го
            Assert.That(_player.Level, Is.EqualTo(2));
            Assert.That(_player.Experience, Is.EqualTo(50)); // Остаток 50
        }

        [Test]
        public void AddExperience_ShouldThrowException_WhenExperienceIsNegative()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _player.AddExperience(-10));
        }

        [Test]
        public async Task AddMoney_ShouldIncreaseMoney_WhenMoneyAmountIsPositive()
        {
            await _player.AddMoney(100);
            Assert.That(_player.Money, Is.EqualTo(100));
        }

        [Test]
        public async Task AddItem_ShouldAddNewItem_WhenItemDoesNotExist()
        {
            var item = new PlayerItem(_player.Id, Guid.NewGuid(), 1);
            await _player.AddItem(item);
            Assert.That(_player.PlayerItems.Count, Is.EqualTo(1));
            Assert.That(_player.PlayerItems.First().ItemId, Is.EqualTo(item.ItemId));
        }

        [Test]
        public async Task AddItem_ShouldIncreaseQuantity_WhenItemAlreadyExists()
        {
            var item = new PlayerItem(_player.Id, Guid.NewGuid(), 1);
            await _player.AddItem(item);
            await _player.AddItem(new PlayerItem(_player.Id, item.ItemId, 3));

            Assert.That(_player.PlayerItems.Count, Is.EqualTo(1));
            Assert.That(_player.PlayerItems.First().Quantity, Is.EqualTo(4));
        }

        [Test]
        public void RemoveItem_ShouldThrowException_WhenItemDoesNotExist()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _player.RemoveItem(Guid.NewGuid(), 1));
        }

        [Test]
        public async Task RemoveItem_ShouldRemoveItem_WhenQuantityIsSufficient()
        {
            var item = new PlayerItem(_player.Id, Guid.NewGuid(), 5);
            await _player.AddItem(item);
            await _player.RemoveItem(item.ItemId, 3);
            Assert.That(_player.PlayerItems.First().Quantity, Is.EqualTo(2));
        }

        [Test]
        public async Task RemoveItem_ShouldThrowException_WhenNotEnoughQuantity()
        {
            var item = new PlayerItem(_player.Id, Guid.NewGuid(), 1);
            await _player.AddItem(item);
            Assert.ThrowsAsync<InvalidOperationException>(() => _player.RemoveItem(item.ItemId, 2));
        }

        [Test]
        public async Task RemoveItem_ShouldRemoveItem_WhenQuantityBecomesZero()
        {
            var item = new PlayerItem(_player.Id, Guid.NewGuid(), 1);
            await _player.AddItem(item);
            await _player.RemoveItem(item.ItemId, 1);
            Assert.That(_player.PlayerItems.Count, Is.EqualTo(0));
        }
    }
}
