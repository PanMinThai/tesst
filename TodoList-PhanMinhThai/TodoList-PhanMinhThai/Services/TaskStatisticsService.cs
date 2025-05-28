using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Models;
using TodoList_PhanMinhThai.Repositories;

namespace TodoList_PhanMinhThai.Services
{
    public class TaskStatisticsService : ITaskStatisticsService
    {
        private readonly ITaskRepository _repository;

        public TaskStatisticsService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskStatistics> GetStatisticsAsync()
        {
            return new TaskStatistics
            {
                InProgressCount = await _repository.GetInProgressCountAsync(),
                CompletedCount = await _repository.GetCompletedCountAsync(),
                CancelledCount = await _repository.GetCancelledCountAsync(),
                TodayTasksCount = await _repository.GetTodayTaskCountAsync(),
                YesterdayTasksCount = await _repository.GetYesterdayTaskCountAsync(),
                ThisWeekTasksCount = await _repository.GetThisWeekTaskCountAsync()
            };
        }
    }
}
