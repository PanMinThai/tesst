using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Tasks.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Services
{
    public interface ITaskStatisticsService
    { 

        Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync(CancellationToken cancellationToken = default);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(CancellationToken cancellationToken = default);
        Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
        Task<int> GetTaskCountByPeriodAsync(DateTimePeriod period, DateTime? from = null, DateTime? to = null);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync(CancellationToken cancellationToken = default);
    }
}
