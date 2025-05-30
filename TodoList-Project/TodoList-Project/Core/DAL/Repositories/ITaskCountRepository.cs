using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Repositories
{
    public interface ITaskCountRepository
    {
        int GetInProgressCount();
        int GetCompletedCount();
        int GetCancelledCount();

        int GetTodayTaskCount();
        int GetYesterdayTaskCount();
        int GetThisWeekTaskCount();
    }
}
