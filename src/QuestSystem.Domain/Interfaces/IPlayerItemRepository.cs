using QuestSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestSystem.Domain.Interfaces
{
    public interface IPlayerItemRepository
    {
        Task<PlayerItem> GetByIdAsync(Guid playerItemId);
        Task<PlayerItem> GetPlayerItemAsync(Guid playerId, Guid itemId);
        Task<IEnumerable<PlayerItem>> GetByPlayerIdAsync(Guid playerId);
        Task AddAsync(PlayerItem playerItem);
        Task Update(PlayerItem playerItem);
        Task Delete(PlayerItem playerItem);
        Task<bool> SaveChangesAsync();
    }
}
