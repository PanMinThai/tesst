using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Repositories;
using TodoList_Project.Features.Tasks.Models;

namespace TodoList_Project.Features.Tasks.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TaskStatistics> GetTaskStatisticsAsync()
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
        public async Task AddTaskAsync(TaskModel model)
        {
            var taskEntity = _mapper.Map<TaskEntity>(model);
            await _repository.AddAsync(taskEntity);
            _mapper.Map(taskEntity, model);
        }
        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskModel>>(entities);
        }
        public async Task UpdateTaskAsync(TaskModel model)
        {
            var existingEntity = await _repository.GetByIdAsync(model.Id);
            if (existingEntity == null)
                throw new KeyNotFoundException($"Task with ID {model.Id} not found");

            _mapper.Map(model, existingEntity);
            await _repository.UpdateAsync(existingEntity);

            model.UpdatedAt = existingEntity.UpdatedAt;
        }
        public async Task DeleteTaskAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public List<TaskModel> GetTasksByDate(DateTime date)
        {
            var taskEntities = _repository.GetTasksByDate(date);

            return _mapper.Map<List<TaskModel>>(taskEntities);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByDateRange(DateTime fromDate, DateTime toDate)
        {
            var taskEntities = await _repository.GetTasksByDateRange(fromDate, toDate);

            return _mapper.Map<List<TaskModel>>(taskEntities);
        }
    }
}
