using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public class GenericRepository<T,Tkey> : IGenericRepository<T,Tkey> where T : class
    {
        protected readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public GenericRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T?> GetByIdAsync(Tkey id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<T>()
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tkey id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var entity = await context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<T>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return predicate != null
                ? await context.Set<T>().CountAsync(predicate)
                : await context.Set<T>().CountAsync();
        }
    }
}
