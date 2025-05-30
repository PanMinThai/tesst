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
        Task<IEnumerable<TaskModel>> ApplyFilters(TaskStatus? status = null, TaskPriority? priority = null, DateTime? date = null);
        Task<IEnumerable<TaskModel>> SearchTasks(string keyword, TaskStatus? status = null, TaskPriority? priority = null);

    }
}
