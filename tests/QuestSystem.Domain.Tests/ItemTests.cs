using NUnit.Framework;
using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Enums;
using System;

namespace QuestSystem.Domain.Tests
{
    [TestFixture]
    public class ItemTests
    {
        [Test]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            string name = "Меч";
            ItemType type = ItemType.Weapon;

            var item = new Item(name, type);

            Assert.That(item.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(item.Name, Is.EqualTo(name));
            Assert.That(item.TypeId, Is.EqualTo((int)type));
            Assert.That(item.Type, Is.EqualTo(type));
        }

        [Test]
        public void Type_ShouldReturnCorrectItemType()
        {
            var item = new Item("Щит", ItemType.Armor);

            var type = item.Type;

            Assert.That(type, Is.EqualTo(ItemType.Armor));
        }

        [Test]
        public void Constructor_ShouldGenerateUniqueIds()
        {
            var item1 = new Item("Лук", ItemType.Weapon);
            var item2 = new Item("Кольцо", ItemType.Armor);

            Assert.That(item2.Id, Is.Not.EqualTo(item1.Id));
        }
    }
}
