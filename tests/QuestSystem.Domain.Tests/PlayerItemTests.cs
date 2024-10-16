using NUnit.Framework;
using QuestSystem.Domain.Entities;
using System;

namespace QuestSystem.Domain.Tests
{
    [TestFixture]
    public class PlayerItemTests
    {
        private PlayerItem _playerItem;

        [SetUp]
        public void Setup()
        {
            _playerItem = new PlayerItem(Guid.NewGuid(), Guid.NewGuid(), 5);
        }

        [Test]
        public async Task AddQuantity_ShouldIncreaseQuantity_WhenAmountIsPositive()
        {
            await _playerItem.AddQuantity(3);
            Assert.That(_playerItem.Quantity, Is.EqualTo(8));
        }

        [Test]
        public void AddQuantity_ShouldThrowException_WhenAmountIsNegative()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _playerItem.AddQuantity(-1));
        }

        [Test]
        public async Task RemoveQuantity_ShouldDecreaseQuantity_WhenAmountIsSufficient()
        {
            await _playerItem.RemoveQuantity(3);
            Assert.That(_playerItem.Quantity, Is.EqualTo(2));
        }

        [Test]
        public void RemoveQuantity_ShouldThrowException_WhenNotEnoughQuantity()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _playerItem.RemoveQuantity(6));
        }

        [Test]
        public void RemoveQuantity_ShouldThrowException_WhenAmountIsNegative()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _playerItem.RemoveQuantity(-1));
        }
    }
}
