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
using TodoList_Project.Core.Utils.Helpers;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.DAL.Repositories.General
{
    public class CategoryRepository : GenericRepository<CategoryEntity, int>, ICategoryRepository
    {
        public CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllWithTaskStatsAsync(DateTimePeriod period = DateTimePeriod.All)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var (startDate, endDate) = DateTimePeriodHelper.GetDateRange(period);

            return await context.Set<CategoryEntity>()
                .Include(c => c.TaskCategories)
                .ThenInclude(tc => tc.Task)
                .Select(c => new CategoryEntity
                {
                    Id = c.Id,
                    Name = c.Name,
                    Icon = c.Icon,
                    Color = c.Color,
                    TaskCategories = c.TaskCategories
                        .Where(tc => period == DateTimePeriod.All ||
                                   (tc.Task.DueDate.HasValue &&
                                    tc.Task.DueDate.Value.Date >= startDate.Date &&
                                    tc.Task.DueDate.Value.Date <= endDate.Date))
                        .ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}