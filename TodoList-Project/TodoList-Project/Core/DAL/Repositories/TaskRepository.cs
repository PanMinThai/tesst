using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync(CancellationToken cancellationToken = default)
        {
            var tasks = await _context.Tasks
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var result = tasks
                .GroupBy(t => t.Priority)
                .Select(g => new
                {
                    Priority = g.Key,
                    StatusCounts = g.GroupBy(t => t.Status)
                                   .ToDictionary(x => x.Key, x => x.Count())
                })
                .ToDictionary(
                    x => x.Priority,
                    x => x.StatusCounts.Any()
                        ? x.StatusCounts
                        : new Dictionary<TaskStatus, int> { { TaskStatus.InProgress, 0 }, { TaskStatus.Completed, 0 }, { TaskStatus.Cancelled, 0 } });

            foreach (var priority in Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>())
            {
                if (!result.ContainsKey(priority))
                {
                    result[priority] = new Dictionary<TaskStatus, int>
                    {
                        { TaskStatus.InProgress, 0 },
                        { TaskStatus.Completed, 0 },
                        { TaskStatus.Cancelled, 0 }
                    };
                }
                else
                {
                    var statusCounts = result[priority];
                    foreach (TaskStatus status in Enum.GetValues(typeof(TaskStatus)))
                    {
                        if (!statusCounts.ContainsKey(status))
                        {
                            statusCounts[status] = 0;
                        }
                    }
                }
            }

            return result;
        }
        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(CancellationToken cancellationToken = default)
        {
            var result = await _context.Tasks
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken)
                .ConfigureAwait(false);


            foreach (TaskStatus status in Enum.GetValues(typeof(TaskStatus)))
            {
                if (!result.ContainsKey(status))
                {
                    result[status] = 0;
                }
            }

            return result;
        }
        public async Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
        {
            var result = await _context.Tasks
                .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date >= fromDate.Date && t.DueDate.Value.Date <= toDate.Date)
                .GroupBy(t => t.DueDate.Value.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(
                    x => x.Date,
                    x => x.Count,
                    cancellationToken)
                .ConfigureAwait(false);

            var allDates = Enumerable.Range(0, (toDate.Date - fromDate.Date).Days + 1)
                .Select(d => fromDate.Date.AddDays(d))
                .ToDictionary(d => d, d => result.ContainsKey(d) ? result[d] : 0);

            return allDates;
        }
        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync(CancellationToken cancellationToken = default)
        {
            // Group tasks by status and count
            var counts = await _context.Tasks
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken)
                .ConfigureAwait(false);

            // Ensure all statuses are included (even if count = 0)
            foreach (TaskStatus status in Enum.GetValues(typeof(TaskStatus)))
            {
                if (!counts.ContainsKey(status))
                {
                    counts[status] = 0;
                }
            }

            return counts;
        }
        public async Task<int> GetTaskCountByPeriodAsync(DateTimePeriod period, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Tasks.AsQueryable();

            switch (period)
            {
                case DateTimePeriod.Today:
                    var todayDate = DateTime.Today; 
                    query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == todayDate);
                    break;
                case DateTimePeriod.Yesterday:
                    var yesterdayDate = DateTime.Today.AddDays(-1); 
                    query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == yesterdayDate);
                    break;
                case DateTimePeriod.ThisWeek:
                    var currentDate = DateTime.Today; 
                    var startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(6);
                    query = query.Where(t => t.DueDate.HasValue && 
                                        t.DueDate.Value.Date >= startOfWeek && 
                                        t.DueDate.Value.Date <= endOfWeek);
                    break;
                case DateTimePeriod.Custom when from.HasValue && to.HasValue:
                    query = query.Where(t => t.DueDate.HasValue && 
                                        t.DueDate.Value.Date >= from.Value.Date && 
                                        t.DueDate.Value.Date <= to.Value.Date);
                    break;
                default:
                    throw new ArgumentException("Invalid period or missing date range");
            }

            return await query.CountAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetFilteredTasksAsync( TaskStatus? status = null, TaskPriority? priority = null, string keyword = null, DateTime? date = null, DateTime? fromDate = null, DateTime? toDate = null, int pageNumber = 1, int pageSize = 10)
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

            // Get total count before pagination
            int totalCount = await query.CountAsync();

            // Apply pagination
            var tasks = await query
                .OrderBy(t => t.DueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return (tasks, totalCount);
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
