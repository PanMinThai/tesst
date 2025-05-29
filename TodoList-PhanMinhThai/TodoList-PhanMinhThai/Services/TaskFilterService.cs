using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Models;

namespace TodoList_PhanMinhThai.Services
{
    public class TaskFilterService : ITaskFilterService
    {
        public IQueryable<TaskModel> ApplyFilters(IQueryable<TaskModel> tasks, Data.Entities.TaskStatus? status, TaskPriority? priority)
        {
            if (status.HasValue)
                tasks = tasks.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                tasks = tasks.Where(t => t.Priority == priority.Value);

            return tasks;
        }
        public IQueryable<TaskModel> SearchTasks(IQueryable<TaskModel> tasks, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return tasks;

            return tasks.Where(t => t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
