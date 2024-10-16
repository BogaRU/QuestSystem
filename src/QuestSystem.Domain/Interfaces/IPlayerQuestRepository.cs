using QuestSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Domain.Interfaces
{
    public interface IPlayerQuestRepository
    {
        Task<PlayerQuest> GetByIdAsync(Guid playerQuestId);
        Task<IEnumerable<PlayerQuest>> GetByPlayerIdAsync(Guid playerId);
        Task AddAsync(PlayerQuest playerQuest);
        Task Update(PlayerQuest playerQuest);
        Task Delete(PlayerQuest playerQuest);
        Task<bool> SaveChangesAsync();


        Task<IEnumerable<Guid>> GetCompletedQuestsAsync(Guid playerId);
        Task<bool> IsQuestAcceptedOrCompletedAsync(Guid playerId, Guid questId);
    }
}
