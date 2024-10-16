using QuestSystem.Domain.Enums;
using System;

namespace QuestSystem.Domain.ValueObjects
{
    public class QuestCondition
    {
        public QuestConditionType Type { get; private set; }
        public int RequiredAmount { get; private set; }
        public ICollection<Guid>? RequiredItemIds { get; private set; }

        public QuestCondition()
        {
            Type = QuestConditionType.VisitLocations;
            RequiredAmount = 0;
            RequiredItemIds = new List<Guid>();
        }

        public QuestCondition(QuestConditionType type, int requiredAmount, ICollection<Guid> requiredItemIds = null) : this()
        {
            Type = type;
            RequiredAmount = requiredAmount;
            RequiredItemIds = requiredItemIds ?? new List<Guid>();
        }
    }
}
