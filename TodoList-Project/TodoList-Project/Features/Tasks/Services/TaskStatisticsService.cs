using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.DAL.Repositories;
using TodoList_Project.Features.Tasks.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus; 

namespace TodoList_Project.Features.Tasks.Services
{
    public class TaskStatisticsService : ITaskStatisticsService
    {
        private readonly ITaskRepository _repository;
        private readonly ILogger<TaskService> _logger;
        public TaskStatisticsService(ITaskRepository repository, ILogger<TaskService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusCountsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching task status counts...");

                var counts = await _repository
                    .GetTaskStatusCountsAsync(cancellationToken)
                    .ConfigureAwait(false);

                _logger.LogInformation("Successfully fetched task status counts");
                return counts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch task status counts");
                throw; // Re-throw để controller xử lý
            }
        }
        public async Task<int> GetTaskCountByPeriodAsync(DateTimePeriod period, DateTime? from = null, DateTime? to = null)
        {
            try
            {
                _logger.LogInformation("Getting task count for period: {Period}", period);  
                if (period == DateTimePeriod.Custom)
                {
                    if (!from.HasValue || !to.HasValue)
                    {
                        throw new ArgumentException("From and To dates are required for custom period");
                    }

                    if (from.Value > to.Value)
                    {
                        throw new ArgumentException("From date cannot be after To date");
                    }
                }

                return await _repository.GetTaskCountByPeriodAsync(period, from, to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting task count for period: {Period}", period);
                throw; 
            }
        }
        public async Task<Dictionary<TaskStatus, int>> GetTaskStatusDistributionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _repository.GetTaskStatusDistributionAsync(cancellationToken);
        }
        public async Task<Dictionary<TaskPriority, Dictionary<TaskStatus, int>>> GetTasksByPriorityAndStatusAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _repository.GetTasksByPriorityAndStatusAsync(cancellationToken);
        }
        public async Task<Dictionary<DateTime, int>> GetTaskCountByDateAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _repository.GetTaskCountByDateAsync(fromDate, toDate, cancellationToken);
        }
    }
}
