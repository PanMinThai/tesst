using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Models;

namespace TodoList_PhanMinhThai.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<Task> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskModel task);
        Task UpdateTaskAsync(TaskModel task);
        Task DeleteTaskAsync(int id);
    }
}
