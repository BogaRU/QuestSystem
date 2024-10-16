using QuestSystem.Domain.Enums;
using QuestSystem.Domain.ValueObjects;
using System;

namespace QuestSystem.Domain.Entities
{
    public class PlayerQuest
    {
        public Guid Id { get; private set; }
        public Guid PlayerId { get; private set; }
        public Guid QuestId { get; private set; }
        public QuestStatus Status { get; internal set; }
        public QuestProgress Progress { get; internal set; }

        public Player? Player { get; internal set; }
        public Quest? Quest { get; internal set; }

        public PlayerQuest(Guid playerId, Guid questId)
        {
            Id = Guid.NewGuid();
            PlayerId = playerId;
            QuestId = questId;
            Status = QuestStatus.Accepted;
            Progress = new QuestProgress();
        }

        public async Task Start()
        {
            if (Status == QuestStatus.Accepted)
            {
                Status = QuestStatus.InProgress;
            }
        }

        public async Task Complete()
        {
            if (Status == QuestStatus.InProgress && Progress.IsCompleted)
            {
                Status = QuestStatus.Completed;
            }
        }

        public async Task Finish()
        {
            if (Status == QuestStatus.Completed)
            {
                Status = QuestStatus.Finished;
            }
        }

        public async Task CheckProgress()
        {
            if (Progress.IsCompleted)
            {
                await Complete();
            }
        }

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public void SetQuest(Quest quest)
        {
            Quest = quest;
        }
    }
}
