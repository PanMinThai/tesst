using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Repositories;
using TodoList_Project.Features.Tasks.Models;

namespace TodoList_Project.Features.Tasks.Services
{
    public class TaskStatisticsService : ITaskStatisticsService
    {
        private readonly ITaskRepository _repository;

        public TaskStatisticsService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return new TaskStatistics
            {
                InProgressCount = await _repository.GetInProgressCountAsync(cancellationToken),
                CompletedCount = await _repository.GetCompletedCountAsync(cancellationToken),
                CancelledCount = await _repository.GetCancelledCountAsync(cancellationToken),
                TodayTasksCount = await _repository.GetTodayTaskCountAsync(cancellationToken),
                YesterdayTasksCount = await _repository.GetYesterdayTaskCountAsync(cancellationToken),
                ThisWeekTasksCount = await _repository.GetThisWeekTaskCountAsync(cancellationToken)
            };
        }
    }
}
