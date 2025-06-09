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

        public async Task<(IEnumerable<TaskModel> Tasks, int TotalCount)> ApplyFilters( TaskStatus? status = null, TaskPriority? priority = null,
            DateTime? date = null, int pageNumber = 1, int pageSize = 10)
        {
            var (taskEntities, totalCount) = await _taskRepository.GetFilteredTasksAsync(
                status: status, 
                priority: priority, 
                date: date, 
                pageNumber: pageNumber, 
                pageSize: pageSize);

            return (_mapper.Map<IEnumerable<TaskModel>>(taskEntities), totalCount);
        }

        public async Task<(IEnumerable<TaskModel> Tasks, int TotalCount)> SearchTasks( string keyword, TaskStatus? status = null, TaskPriority? priority = null, int pageNumber = 1, int pageSize = 10)
        {
            var (taskEntities, totalCount) = await _taskRepository.GetFilteredTasksAsync(
                status: status, 
                priority: priority, 
                keyword: keyword, 
                pageNumber: pageNumber, 
                pageSize: pageSize);

            return (_mapper.Map<IEnumerable<TaskModel>>(taskEntities), totalCount);
        }

    }
}
