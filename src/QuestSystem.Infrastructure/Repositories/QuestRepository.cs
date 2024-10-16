using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Infrastructure.Repositories
{
    public class QuestRepository : IQuestRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Quest quest)
        {
            await _context.Quests.AddAsync(quest);
        }

        public async Task Delete(Quest quest)
        {
            _context.Quests.Remove(quest);
        }

        public async Task<IEnumerable<Quest>> GetAllAsync()
        {
            return await _context.Quests
                .Include(q => q.Conditions)
                .Include(q => q.Rewards)
                .ToListAsync();
        }

        public async Task<Quest> GetByIdAsync(Guid questId)
        {
            return await _context.Quests
                .Include(q => q.Conditions)
                .Include(q => q.Rewards)
                .FirstOrDefaultAsync(q => q.Id == questId);
        }

        public async Task Update(Quest quest)
        {
            _context.Quests.Update(quest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
