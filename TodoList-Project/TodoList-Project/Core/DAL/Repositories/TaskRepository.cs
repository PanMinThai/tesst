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
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.DAL.Repositories    
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context ;

        public TaskRepository(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Implementation of IGenericRepository<TaskEntity>
        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            using var context = _context.CreateDbContext();
            return await context.Tasks.ToListAsync();
        }

        public async Task<TaskEntity> GetByIdAsync(object id)
        {
            if (id is not int taskId)
                throw new ArgumentException("ID must be an integer");

            using var context = _context.CreateDbContext();
            return await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId).ConfigureAwait(false);
        }

        public async Task AddAsync(TaskEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using var context = _context.CreateDbContext();
            await context.Tasks.AddAsync(entity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(TaskEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using var context = _context.CreateDbContext();
            context.Tasks.Update(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            using var context = _context.CreateDbContext();
            var entity = await context.Tasks.FirstOrDefaultAsync(t => t.Id == id).ConfigureAwait(false);
            if (entity != null)
            {
                context.Tasks.Remove(entity);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        #endregion

        #region Implementation of ITaskRepository
        public async Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync( DateTimePeriod period = DateTimePeriod.ThisMonth, DateTime? customStartDate = null, DateTime? customEndDate = null)
        {
            var (startDate, endDate) = DateTimePeriodHelper.GetDateRange(period, customStartDate, customEndDate);

            using var context = _context.CreateDbContext();

            var tasks = await context.Tasks
                .AsNoTracking()
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value.Date >= startDate &&
                           t.DueDate.Value.Date <= endDate)
                .ToListAsync()
                .ConfigureAwait(false);

            var emptyStatusDictionary = Enum.GetValues(typeof(TaskStatus))
                .Cast<TaskStatus>()
                .ToDictionary(s => s, s => 0);

            var result = tasks
                .GroupBy(t => t.Priority)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(t => t.Status)
                         .ToDictionary(
                             sg => sg.Key,
                             sg => sg.Count()));

            foreach (TaskPriority priority in Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>())
            {
                if (!result.TryGetValue(priority, out var statusDict))
                {
                    result[priority] = new Dictionary<TaskStatus, int>(emptyStatusDictionary);
                }
                else
                {
                    foreach (var status in emptyStatusDictionary.Keys)
                    {
                        if (!statusDict.ContainsKey(status))
                        {
                            statusDict[status] = 0;
                        }
                    }
                }
            }

            return result;
        }

        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync( DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null)
        {
            var (startDate, endDate) = DateTimePeriodHelper.GetDateRange(period, customStartDate, customEndDate);

            using var context = _context.CreateDbContext();

            var allStatuses = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().ToList();

            var statusCounts = await context.Tasks
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value.Date >= startDate &&
                           t.DueDate.Value.Date <= endDate)
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync()  
                .ConfigureAwait(false);

            var result = allStatuses.ToDictionary(status => status, _ => 0);

            foreach (var item in statusCounts)
            {
                result[item.Status] = item.Count;
            }

            return result;
        }

        public async Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync( DateTimePeriod period = DateTimePeriod.ThisMonth, DateTime? customStartDate = null, DateTime? customEndDate = null)
        {
            var (startDate, endDate) = DateTimePeriodHelper.GetDateRange(period, customStartDate, customEndDate);

            startDate = startDate.Date;
            endDate = endDate.Date;

            using var context = _context.CreateDbContext();

            var dbResults = await context.Tasks
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value.Date >= startDate &&
                           t.DueDate.Value.Date <= endDate)
                .GroupBy(t => t.DueDate.Value.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync()
                .ConfigureAwait(false);

            var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                .Select(offset => startDate.AddDays(offset))
                .ToDictionary(date => date, _ => 0);

            foreach (var item in dbResults)
            {
                if (allDates.ContainsKey(item.Date))
                {
                    allDates[item.Date] = item.Count;
                }
            }

            return allDates;
        }

        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync()
        {
            using var context = _context.CreateDbContext();

            var counts = await context.Tasks
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count)
                .ConfigureAwait(false);

            foreach (TaskStatus status in Enum.GetValues(typeof(TaskStatus)))
            {
                if (!counts.ContainsKey(status))
                {
                    counts[status] = 0;
                }
            }

            return counts;
        }

        public async Task<int> GetTaskCountByPeriodAsync(DateTimePeriod period, DateTime? from = null, DateTime? to = null)
        {
            using var context = _context.CreateDbContext();

            var query = context.Tasks.AsQueryable();

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

            return await query.CountAsync().ConfigureAwait(false);
        }


        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetFilteredTasksAsync( TaskStatus? status = null, TaskPriority? priority = null, string keyword = null, DateTime? date = null, DateTime? fromDate = null, DateTime? toDate = null, int pageNumber = 1,   int pageSize = 10)
        {
            using var context = _context.CreateDbContext();

            var query = context.Tasks.AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(t => t.Title.Contains(keyword));

            if (date.HasValue)
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == date.Value.Date);

            if (fromDate.HasValue && toDate.HasValue)
                query = query.Where(t => t.DueDate >= fromDate && t.DueDate <= toDate);

            int totalCount = await query.CountAsync().ConfigureAwait(false);

            var tasks = await query
                .OrderBy(t => t.DueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return (tasks, totalCount);
        }
        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetTodayUpdatedCompletedAndCancelledTasks(int pageNumber = 1, int pageSize = 10)
        {
            using var context = _context.CreateDbContext();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var query = context.Tasks
                .Where(t =>
                    (t.Status == TaskStatus.Completed || t.Status == TaskStatus.Cancelled) &&
                    t.UpdatedAt >= today && t.UpdatedAt < tomorrow);

            int totalCount = await query.CountAsync().ConfigureAwait(false);

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

    }
}
