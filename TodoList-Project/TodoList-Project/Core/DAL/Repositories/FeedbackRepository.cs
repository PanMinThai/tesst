using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;

namespace TodoList_Project.Core.DAL.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserFeedbackEntity>> GetAllAsync()
        {
            return await _context.UserFeedbacks.ToListAsync();
        }

        public async Task<UserFeedbackEntity> GetByIdAsync(object id)
        {
            return await _context.UserFeedbacks.FindAsync(id);
        }

        public async Task AddAsync(UserFeedbackEntity entity)
        {
            _context.UserFeedbacks.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserFeedbackEntity entity)
        {
            _context.UserFeedbacks.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.UserFeedbacks.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetUsedMessagesAsync(string actionType, Tone tone, DateTime date)
        {
            var nextDay = date.AddDays(1);
            return await _context.UserFeedbacks
                .Where(f => f.ActionType == actionType
                         && f.Tone == tone
                         && f.CreatedAt >= date && f.CreatedAt < nextDay)
                .Select(f => f.Message)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetUsedIconsAsync(string actionType, Tone tone, DateTime date)
        {
            var nextDay = date.AddDays(1);
            return await _context.UserFeedbacks
                .Where(f => f.ActionType == actionType
                         && f.Tone == tone
                         && f.CreatedAt >= date && f.CreatedAt < nextDay)
                .Select(f => f.ImagePath)
                .Distinct()
                .ToListAsync();
        }
    }

}
