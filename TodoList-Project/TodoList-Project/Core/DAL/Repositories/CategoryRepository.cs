using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Features.Categories.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        public CategoryRepository(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
            using var context = _context.CreateDbContext();
            return await context.Categories.ToListAsync();
        }

        public Task<CategoryEntity> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CategoryEntity entity)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<CategoryEntity>> GetAllWithTaskStatsAsync()
        {
            using var context = _context.CreateDbContext();
            return await context.Categories
                .Include(c => c.TaskCategories)
                .ThenInclude(tc => tc.Task) 
                .ToListAsync();
        }
    }
}
