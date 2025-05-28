using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data;
using TodoList_PhanMinhThai.Data.Entities;

namespace TodoList_PhanMinhThai.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Implementation of IGenericRepository<TaskEntity>
        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await _context.Tasks
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<TaskEntity> GetByIdAsync(object id)
        {
            if (id is not int taskId)
                throw new ArgumentException("ID must be an integer");

            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId)
                .ConfigureAwait(false);
        }

        public async Task AddAsync(TaskEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Tasks.AddAsync(entity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(TaskEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(object id)
        {
            if (id is not int taskId)
                throw new ArgumentException("ID must be an integer");

            var entity = await GetByIdAsync(taskId).ConfigureAwait(false);
            if (entity != null)
            {
                _context.Tasks.Remove(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        #endregion

        #region Implementation of ITaskRepository
        public async Task<int> GetInProgressCountAsync()
        {
            return await _context.Tasks
                .CountAsync(t => t.Status == Data.Entities.TaskStatus.InProgress)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCompletedCountAsync()
        {
            return await _context.Tasks
                .CountAsync(t => t.Status == Data.Entities.TaskStatus.Completed)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCancelledCountAsync()
        {
            return await _context.Tasks
                .CountAsync(t => t.Status == Data.Entities.TaskStatus.Cancelled)
                .ConfigureAwait(false);
        }

        public async Task<int> GetTodayTaskCountAsync()
        {
            var today = DateTime.Today;
            return await _context.Tasks
                .CountAsync(t => t.DueDate.HasValue && t.DueDate.Value.Date == today)
                .ConfigureAwait(false);
        }

        public async Task<int> GetYesterdayTaskCountAsync()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            return await _context.Tasks
                .CountAsync(t => t.DueDate.HasValue && t.DueDate.Value.Date == yesterday)
                .ConfigureAwait(false);
        }

        public async Task<int> GetThisWeekTaskCountAsync()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            return await _context.Tasks
                .CountAsync(t => t.DueDate >= startOfWeek && t.DueDate <= endOfWeek)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksDueThisWeekAsync()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            return await _context.Tasks
                .Where(t => t.DueDate >= startOfWeek && t.DueDate <= endOfWeek)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }
        #endregion

        #region IDisposable Implementation
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)

                    _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}