using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Models;
using TodoList_PhanMinhThai.Data.Enums;
using TaskStatus = TodoList_PhanMinhThai.Data.Enums.TaskStatus;


namespace TodoList_PhanMinhThai.Repositories
{
    public interface ITaskRepository : IGenericRepository<TaskEntity>,ITaskStatisticsRepository
    {
        //
        Task<IEnumerable<TaskEntity>> GetTasksDueThisWeekAsync();
        IQueryable<TaskEntity> GetTasksByDate(DateTime date);
        Task<IEnumerable<TaskEntity>> GetTasksByDateRange(DateTime from, DateTime to);
        Task<IEnumerable<TaskEntity>> GetFilteredTasksAsync(TaskStatus? status = null, TaskPriority? priority = null, string keyword = null, DateTime? date = null, DateTime? fromDate = null, DateTime? toDate = null);
    }
}
