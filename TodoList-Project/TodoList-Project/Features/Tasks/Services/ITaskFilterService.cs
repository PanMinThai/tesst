using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Tasks.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Services
{
    public interface ITaskFilterService
    {
        Task<(IEnumerable<TaskModel> Tasks, int TotalCount)> ApplyFilters( TaskStatus? status, TaskPriority? priority, DateTime? date, int pageNumber = 1, int pageSize = 10);

        Task<(IEnumerable<TaskModel> Tasks, int TotalCount)> SearchTasks( string keyword, TaskStatus? status, TaskPriority? priority, int pageNumber = 1, int pageSize = 10);

    }
}
