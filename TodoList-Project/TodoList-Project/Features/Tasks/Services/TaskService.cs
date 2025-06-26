using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Repositories.Interfaces;
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
            var taskEntity = await _repository.GetByIdAsync(model.Id);
            _mapper.Map(model, taskEntity);
            await _repository.UpdateAsync(taskEntity);

            model.UpdatedAt = taskEntity.UpdatedAt;
        }
        public async Task DeleteTaskAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
