using QuestSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Application.Interfaces
{
    public interface IPlayerService
    {
        Task<Player> GetPlayerByIdAsync(Guid playerId);
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task CreatePlayerAsync(Player player);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(Guid playerId);
    }
}
