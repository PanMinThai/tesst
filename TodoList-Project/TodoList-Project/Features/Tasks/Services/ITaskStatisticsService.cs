using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Tasks.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Services
{
    public interface ITaskStatisticsService
    {
        Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(CancellationToken cancellationToken = default);
        Task<TaskStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default);
    }
}
