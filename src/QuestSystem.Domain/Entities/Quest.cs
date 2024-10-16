using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using QuestSystem.Domain.ValueObjects;

namespace QuestSystem.Domain.Entities
{
    public class Quest
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        [NotMapped]
        public ICollection<QuestCondition> Conditions { get; private set; }
        public ICollection<QuestReward> Rewards { get; private set; }
        public int MinimumLevel { get; private set; }
        public ICollection<Guid> RequiredCompletedQuests { get; private set; }

        public Quest(string title, string description, int minimumLevel)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            MinimumLevel = minimumLevel;
            Conditions = new List<QuestCondition>();
            Rewards = new List<QuestReward>();
            RequiredCompletedQuests = new List<Guid>();
        }

        public async Task AddCondition(QuestCondition condition)
        {
            Conditions.Add(condition);
        }

        public async Task AddReward(QuestReward reward)
        {
            Rewards.Add(reward);

        }

        public async Task AddRequiredCompletedQuest(Guid questId)
        {
            RequiredCompletedQuests.Add(questId);
        }
    }
}
