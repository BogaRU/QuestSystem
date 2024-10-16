using QuestSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Domain.Interfaces
{
    public interface IQuestRepository
    {
        Task<Quest> GetByIdAsync(Guid questId);
        Task<IEnumerable<Quest>> GetAllAsync();
        Task AddAsync(Quest quest);
        Task Update(Quest quest);
        Task Delete(Quest quest);
        Task<bool> SaveChangesAsync();
    }
}
