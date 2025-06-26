using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.Enums;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public interface ITaskStatisticsRepository
    {
        // Task statistics methods
        Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync(DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null);
        Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync(DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null);
        Task<int> GetTaskCountByPeriodAsync(DateTimePeriod period, DateTime? from = null, DateTime? to = null);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync();
        Task<int> CountByStatusTodayAsync(TaskStatus status);
    }
}
