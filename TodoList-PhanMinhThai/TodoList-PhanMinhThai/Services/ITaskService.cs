using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Models;

namespace TodoList_PhanMinhThai.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task AddTaskAsync(TaskModel task);
        Task UpdateTaskAsync(TaskModel task);
        Task DeleteTaskAsync(int id);
        Task<TaskStatistics> GetTaskStatisticsAsync();
        Task<List<TaskModel>> GetTasksByDate(DateTime date);
        Task<List<TaskModel>> GetTasksByDateRange(DateTime fromDate, DateTime toDate);
    }
}
