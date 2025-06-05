using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.DAL.Repositories;
using TodoList_Project.Features.Tasks.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Services
{
    public class TaskFilterService : ITaskFilterService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskFilterService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskModel>> ApplyFilters(TaskStatus? status = null, TaskPriority? priority = null, DateTime? date = null)
        {
            var taskEntities = await _taskRepository.GetFilteredTasksAsync(status, priority, date: date);
            return _mapper.Map<List<TaskModel>>(taskEntities);
        }

        public async Task<IEnumerable<TaskModel>> SearchTasks(string keyword, TaskStatus? status = null, TaskPriority? priority = null)
        {
            var taskEntities = await _taskRepository.GetFilteredTasksAsync(status, priority, keyword);
            return _mapper.Map<List<TaskModel>>(taskEntities);
        }
    }
}
