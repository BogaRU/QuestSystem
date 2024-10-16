using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlayerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Player player)
        {
            await _context.Players.AddAsync(player);
        }

        public async Task Delete(Player player)
        {
            _context.Players.Remove(player);
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<Player> GetByIdAsync(Guid playerId)
        {
            return await _context.Players
                .Include(p => p.PlayerQuests)
                .FirstOrDefaultAsync(p => p.Id == playerId);
        }

        public async Task Update(Player player)
        {
            _context.Players.Update(player);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
