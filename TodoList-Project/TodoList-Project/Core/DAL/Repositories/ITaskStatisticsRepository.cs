using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Repositories
{
    public interface ITaskStatisticsRepository
    {
        Task<int> GetInProgressCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetCompletedCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetCancelledCountAsync(CancellationToken cancellationToken = default);

        Task<int> GetTodayTaskCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetYesterdayTaskCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetThisWeekTaskCountAsync(CancellationToken cancellationToken = default);
    }
}
