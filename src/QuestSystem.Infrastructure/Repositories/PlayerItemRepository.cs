using Microsoft.EntityFrameworkCore;
using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Enums;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestSystem.Infrastructure.Repositories
{
    public class PlayerItemRepository : IPlayerItemRepository
    {
        private readonly ApplicationDbContext _context;

        public PlayerItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PlayerItem playerItem)
        {
            await _context.PlayerItems.AddAsync(playerItem);
        }

        public async Task Delete(PlayerItem playerItem)
        {
            _context.PlayerItems.Remove(playerItem);
        }

        public async Task<PlayerItem> GetPlayerItemAsync(Guid playerId, Guid itemId)
        {
            return await _context.PlayerItems
                .FirstOrDefaultAsync(pi => pi.PlayerId == playerId && pi.ItemId == itemId);
        }

        public async Task<IEnumerable<PlayerItem>> GetByPlayerIdAsync(Guid playerId)
        {
            return await _context.PlayerItems
                .Include(pi => pi.Item)
                .Where(pi => pi.PlayerId == playerId)
                .ToListAsync();
        }

        public async Task<PlayerItem> GetByIdAsync(Guid playerItemId)
        {
            return await _context.PlayerItems
                .Include(pi => pi.Item)
                .FirstOrDefaultAsync(pi => pi.Id == playerItemId);
        }

        public async Task Update(PlayerItem playerItem)
        {
            _context.PlayerItems.Update(playerItem);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
