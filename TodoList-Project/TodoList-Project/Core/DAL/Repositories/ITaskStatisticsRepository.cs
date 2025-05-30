using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Repositories
{
    public interface ITaskStatisticsRepository
    {
        Task<int> GetInProgressCountAsync();
        Task<int> GetCompletedCountAsync();
        Task<int> GetCancelledCountAsync();

        Task<int> GetTodayTaskCountAsync();
        Task<int> GetYesterdayTaskCountAsync();
        Task<int> GetThisWeekTaskCountAsync();
    }
}
