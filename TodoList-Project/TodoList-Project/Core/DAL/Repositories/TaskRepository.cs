using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.DAL.Repositories
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

            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId).ConfigureAwait(false);
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

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id).ConfigureAwait(false);
            if (entity != null)
            {
                _context.Tasks.Remove(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        #endregion

        #region Implementation of ITaskRepository
        public async Task<int> GetInProgressCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .CountAsync(t => t.Status == TaskStatus.InProgress,cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCompletedCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .CountAsync(t => t.Status == TaskStatus.Completed, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCancelledCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .CountAsync(t => t.Status == TaskStatus.Cancelled, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetTodayTaskCountAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.Today;
            return await _context.Tasks
                .CountAsync(t => t.DueDate.HasValue && t.DueDate.Value.Date == today, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetYesterdayTaskCountAsync(CancellationToken cancellationToken = default)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            return await _context.Tasks
                .CountAsync(t => t.DueDate.HasValue && t.DueDate.Value.Date == yesterday, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetThisWeekTaskCountAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            return await _context.Tasks
                .CountAsync(t => t.DueDate >= startOfWeek && t.DueDate <= endOfWeek, cancellationToken)
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
        public IQueryable<TaskEntity> GetTasksByDate(DateTime date)
        {
            return _context.Tasks
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == date.Date);
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByDateRange(DateTime from, DateTime to)
        {
            var query = _context.Tasks
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value.Date >= from.Date &&
                           t.DueDate.Value.Date <= to.Date);
            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<TaskEntity>> GetFilteredTasksAsync( TaskStatus? status = null, TaskPriority? priority = null,
    string keyword = null, DateTime? date = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Tasks.AsQueryable();

            // Filter by Status
            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            // Filter by Priority
            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            // Filter by Title
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(t => t.Title.Contains(keyword));

            // Filter by specific date
            if (date.HasValue)
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == date.Value.Date);

            // Filter by time period
            if (fromDate.HasValue && toDate.HasValue)
                query = query.Where(t => t.DueDate >= fromDate && t.DueDate <= toDate);

            return await query.AsNoTracking().ToListAsync().ConfigureAwait(false); 
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
