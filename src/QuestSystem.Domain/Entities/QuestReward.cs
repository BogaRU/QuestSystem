using System;

namespace QuestSystem.Domain.Entities
{
    public class QuestReward
    {
        public Guid Id { get; private set; }
        public RewardType Type { get; private set; }
        public int Amount { get; private set; }
        public ICollection<Guid> ItemIds { get; private set; }

        public QuestReward(RewardType type, int amount, ICollection<Guid> itemIds = null)
        {
            Id = Guid.NewGuid();
            Type = type;
            Amount = amount;
            ItemIds = itemIds ?? new List<Guid>();
        }
    }

    public enum RewardType
    {
        Experience,
        Money,
        Item
    }
}
