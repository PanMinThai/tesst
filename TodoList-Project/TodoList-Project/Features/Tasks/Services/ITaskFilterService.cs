using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Tasks.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Services
{
    public interface ITaskFilterService
    {
        Task<(IEnumerable<TaskModel> Tasks, int TotalCount)> ApplyFilters( TaskStatus? status =null, TaskPriority? priority=null, DateTime? date = null, int pageNumber = 1, int pageSize = 10);

        Task<(IEnumerable<TaskModel> Tasks, int TotalCount)> SearchTasks( string keyword, TaskStatus? status, TaskPriority? priority, int pageNumber = 1, int pageSize = 10);
        Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetTodayUpdatedCompletedAndCancelledTasks(int pageNumber = 1, int pageSize = 10);
    }
}
