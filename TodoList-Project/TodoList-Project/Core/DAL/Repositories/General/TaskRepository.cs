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
    public class TaskRepository : GenericRepository<TaskEntity, int>, ITaskRepository
    {
        public TaskRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        #region Implementation of ITaskRepository
        public async Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync(DateTimePeriod period = DateTimePeriod.ThisMonth, DateTime? customStartDate = null, DateTime? customEndDate = null)
        {
            var (startDate, endDate) = DateTimePeriodHelper.GetDateRange(period, customStartDate, customEndDate);

            await using var context = await _contextFactory.CreateDbContextAsync();

            var tasks = await context.Set<TaskEntity>()
                .AsNoTracking()
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value.Date >= startDate &&
                           t.DueDate.Value.Date <= endDate)
                .ToListAsync();

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

        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null)
        {
            var (startDate, endDate) = DateTimePeriodHelper.GetDateRange(period, customStartDate, customEndDate);

            await using var context = await _contextFactory.CreateDbContextAsync();

            var allStatuses = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().ToList();

            var statusCounts = await context.Set<TaskEntity>()
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value.Date >= startDate &&
                           t.DueDate.Value.Date <= endDate)
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = allStatuses.ToDictionary(status => status, _ => 0);

            foreach (var item in statusCounts)
            {
                result[item.Status] = item.Count;
            }

            return result;
        }

        public async Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync(DateTimePeriod period = DateTimePeriod.ThisMonth, DateTime? customStartDate = null, DateTime? customEndDate = null)
        {
            var (startDate, endDate) = DateTimePeriodHelper.GetDateRange(period, customStartDate, customEndDate);

            startDate = startDate.Date;
            endDate = endDate.Date;

            await using var context = await _contextFactory.CreateDbContextAsync();

            var dbResults = await context.Set<TaskEntity>()
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value.Date >= startDate &&
                           t.DueDate.Value.Date <= endDate)
                .GroupBy(t => t.DueDate.Value.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

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
            await using var context = await _contextFactory.CreateDbContextAsync();

            var counts = await context.Set<TaskEntity>()
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);

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
            await using var context = await _contextFactory.CreateDbContextAsync();

            var query = context.Set<TaskEntity>().AsQueryable();

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

            return await query.CountAsync();
        }

        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetFilteredTasksAsync(
            TaskStatus? status = null,
            TaskPriority? priority = null,
            string keyword = null,
            DateTime? date = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var query = context.Set<TaskEntity>().AsQueryable();

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

            int totalCount = await query.CountAsync();

            var tasks = await query
                .OrderBy(t => t.DueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (tasks, totalCount);
        }

        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetTodayUpdatedCompletedAndCancelledTasks(int pageNumber = 1, int pageSize = 10)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var query = context.Set<TaskEntity>()
                .Where(t =>
                    (t.Status == TaskStatus.Completed || t.Status == TaskStatus.Cancelled) &&
                    t.UpdatedAt >= today && t.UpdatedAt < tomorrow);

            int totalCount = await query.CountAsync();

            var tasks = await query
                .OrderByDescending(t => t.UpdatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (tasks, totalCount);
        }

        public async Task<int> CountByStatusTodayAsync(TaskStatus status)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await context.Set<TaskEntity>()
                .CountAsync(t =>
                    t.Status == status &&
                    t.UpdatedAt >= today &&
                    t.UpdatedAt < tomorrow);
        }
        #endregion
    }
}