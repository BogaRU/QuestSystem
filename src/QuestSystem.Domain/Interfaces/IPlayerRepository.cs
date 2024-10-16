using QuestSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Domain.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player> GetByIdAsync(Guid playerId);
        Task<IEnumerable<Player>> GetAllAsync();
        Task AddAsync(Player player);
        Task Update(Player player);
        Task Delete(Player player);
        Task<bool> SaveChangesAsync();
    }
}
