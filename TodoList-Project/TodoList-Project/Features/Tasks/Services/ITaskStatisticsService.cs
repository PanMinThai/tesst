using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Tasks.Models;

namespace TodoList_Project.Features.Tasks.Services
{
    public interface ITaskStatisticsService
    {
        Task<TaskStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default);
    }
}
