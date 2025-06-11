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
        Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
        Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync(CancellationToken cancellationToken = default);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(CancellationToken cancellationToken = default);
        Task<int> GetInProgressCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetCompletedCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetCancelledCountAsync(CancellationToken cancellationToken = default);

        Task<int> GetTodayTaskCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetYesterdayTaskCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetThisWeekTaskCountAsync(CancellationToken cancellationToken = default);
    }
}
