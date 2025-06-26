using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.DAL.Repositories.Interfaces;

namespace TodoList_Project.Core.DAL.Repositories.General
{
    public class FeedbackRepository : GenericRepository<UserFeedbackEntity, int>, IFeedbackRepository
    {
        public FeedbackRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<List<string>> GetUsedMessagesAsync(string actionType, Tone tone, DateTime date)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var nextDay = date.AddDays(1);

            return await context.Set<UserFeedbackEntity>()
                .Where(f => f.ActionType == actionType
                         && f.Tone == tone
                         && f.CreatedAt >= date
                         && f.CreatedAt < nextDay)
                .Select(f => f.Message)
                .Distinct()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<string>> GetUsedIconsAsync(string actionType, Tone tone, DateTime date)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var nextDay = date.AddDays(1);

            return await context.Set<UserFeedbackEntity>()
                .Where(f => f.ActionType == actionType
                         && f.Tone == tone
                         && f.CreatedAt >= date
                         && f.CreatedAt < nextDay)
                .Select(f => f.ImagePath)
                .Distinct()
                .AsNoTracking()
                .ToListAsync();
        }

    }
}