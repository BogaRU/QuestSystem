using QuestSystem.Application.DTOs;
using QuestSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Application.Interfaces
{
    public interface IQuestService
    {
        Task<IEnumerable<Quest>> GetAvailableQuestsAsync(Guid playerId);
        Task<PlayerQuest> AcceptQuestAsync(Guid playerId, Guid questId);
        Task<PlayerQuest> UpdateQuestProgressAsync(Guid playerQuestId, QuestProgressUpdateDto progressUpdate);
        Task FinishQuestAsync(Guid playerQuestId);
    }
}
