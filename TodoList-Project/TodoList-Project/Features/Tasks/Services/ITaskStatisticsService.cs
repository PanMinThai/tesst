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

        Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync(DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null);
        Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync(DateTimePeriod period, DateTime? customStartDate = null, DateTime? customEndDate = null);
        Task<int> GetTaskCountByPeriodAsync(DateTimePeriod period, DateTime? from = null, DateTime? to = null);
        Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync();
    }
}
