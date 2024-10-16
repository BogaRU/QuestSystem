using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestSystem.Domain.Enums;

namespace QuestSystem.Infrastructure.Repositories
{
    public class PlayerQuestRepository : IPlayerQuestRepository
    {
        private readonly ApplicationDbContext _context;

        public PlayerQuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PlayerQuest playerQuest)
        {
            await _context.PlayerQuests.AddAsync(playerQuest);
        }

        public async Task Delete(PlayerQuest playerQuest)
        {
            _context.PlayerQuests.Remove(playerQuest);
        }

        public async Task<IEnumerable<PlayerQuest>> GetByPlayerIdAsync(Guid playerId)
        {
            return await _context.PlayerQuests
                .Include(pq => pq.Quest)
                .Where(pq => pq.PlayerId == playerId)
                .ToListAsync();
        }

        public async Task<PlayerQuest> GetByIdAsync(Guid playerQuestId)
        {
            return await _context.PlayerQuests
                .Include(pq => pq.Quest)
                .FirstOrDefaultAsync(pq => pq.Id == playerQuestId);
        }

        public async Task Update(PlayerQuest playerQuest)
        {
            _context.PlayerQuests.Update(playerQuest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }


        public async Task<IEnumerable<Guid>> GetCompletedQuestsAsync(Guid playerId)
        {
            return await _context.PlayerQuests
                .Where(pq => pq.PlayerId == playerId && pq.Status == QuestStatus.Finished)
                .Select(pq => pq.QuestId)
                .ToListAsync();
        }

        public async Task<bool> IsQuestAcceptedOrCompletedAsync(Guid playerId, Guid questId)
        {
            return await _context.PlayerQuests
                .AnyAsync(pq => pq.PlayerId == playerId &&
                                pq.QuestId == questId &&
                                (pq.Status == QuestStatus.Accepted || pq.Status == QuestStatus.InProgress || pq.Status == QuestStatus.Completed));
        }
    }
}
