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
        public IEnumerable<TaskModel> ApplyFilters(IEnumerable<TaskModel> tasks, Data.Entities.TaskStatus? status, TaskPriority? priority)
        {
            if (status.HasValue)
                tasks = tasks.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                tasks = tasks.Where(t => t.Priority == priority.Value);

            return tasks;
        }
    }
}
