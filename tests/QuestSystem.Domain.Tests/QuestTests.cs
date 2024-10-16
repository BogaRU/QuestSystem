using NUnit.Framework;
using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace QuestSystem.Domain.Tests
{
    [TestFixture]
    public class QuestTests
    {
        [Test]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            string title = "Первый Квест";
            string description = "Описание первого квеста";
            int minimumLevel = 5;

            var quest = new Quest(title, description, minimumLevel);

            Assert.That(quest.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(title, Is.EqualTo(quest.Title));
            Assert.That(quest.Description, Is.EqualTo(description));
            Assert.That(quest.MinimumLevel, Is.EqualTo(minimumLevel));
            Assert.That(quest.Conditions, Is.Not.Null);
            Assert.IsEmpty(quest.Conditions);
            Assert.IsNotNull(quest.Rewards);
            Assert.IsEmpty(quest.Rewards);
            Assert.IsNotNull(quest.RequiredCompletedQuests);
            Assert.IsEmpty(quest.RequiredCompletedQuests);
        }

        [Test]
        public async Task AddCondition_ShouldAddConditionToQuest()
        {
            var quest = new Quest("Квест", "Описание", 1);
            var condition = new QuestCondition(QuestConditionType.CollectItems, 10, new List<Guid> { Guid.NewGuid() });

            await quest.AddCondition(condition);

            Assert.Contains(condition, (System.Collections.ICollection)quest.Conditions);
            Assert.That(quest.Conditions.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task AddReward_ShouldAddRewardToQuest()
        {
            var quest = new Quest("Квест", "Описание", 1);
            var reward = new QuestReward(RewardType.Experience, 100);

            await quest.AddReward(reward);

            Assert.Contains(reward, (System.Collections.ICollection)quest.Rewards);
            Assert.That(quest.Rewards.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task AddRequiredCompletedQuest_ShouldAddQuestId()
        {
            var quest = new Quest("Квест", "Описание", 1);
            var requiredQuestId = Guid.NewGuid();

            await quest.AddRequiredCompletedQuest(requiredQuestId);

            Assert.Contains(requiredQuestId, (System.Collections.ICollection)quest.RequiredCompletedQuests);
            Assert.That(quest.RequiredCompletedQuests.Count, Is.EqualTo(1));
        }
    }
}
