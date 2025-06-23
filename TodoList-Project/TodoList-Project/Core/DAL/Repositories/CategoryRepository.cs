using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.Utils.Helpers;
using TodoList_Project.Features.Categories.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public Task AddAsync(CategoryEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Categories.ToListAsync();
        }

        public async Task<CategoryEntity> GetByIdAsync(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(CategoryEntity entity)
        {
            var context = _contextFactory.CreateDbContext();
            context.Categories.Update(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<IEnumerable<CategoryEntity>> GetAllWithTaskStatsAsync(DateTimePeriod period = DateTimePeriod.All)
        {
            using var context = _contextFactory.CreateDbContext();

            var (startDate, endDate) = DateTimePeriodHelper.GetDateRange(period);

            return await context.Categories
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
                .ToListAsync();
        }
    }
}