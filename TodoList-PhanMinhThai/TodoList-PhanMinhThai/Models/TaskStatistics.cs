using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_PhanMinhThai.Models
{
    public class TaskStatistics
    {
        public int TotalTasks { get; set; }
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public int CancelledCount { get; set; }
        public int TodayTasksCount { get; set; }
        public int YesterdayTasksCount { get; set; }
        public int ThisWeekTasksCount { get; set; }

        public double CompletionRate => TotalTasks > 0 ? (CompletedCount / (double)TotalTasks) * 100 : 0;
    }
}
