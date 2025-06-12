using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.Enums;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.DAL.Repositories
{
    public interface ITaskStatisticsRepository
    {
        // Task statistics methods
        Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
        Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync(CancellationToken cancellationToken = default);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(CancellationToken cancellationToken = default);
        Task<int> GetTaskCountByPeriodAsync(DateTimePeriod period, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync(CancellationToken cancellationToken = default);
    }
}
