using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Models;

namespace TodoList_PhanMinhThai.Repositories
{
    public interface ITaskRepository : IGenericRepository<TaskEntity>,ITaskStatisticsRepository
    {
        //
        Task<IEnumerable<TaskEntity>> GetTasksDueThisWeekAsync();
        Task<IQueryable<TaskEntity>> GetTasksByDateAsync(DateTime date);
        Task<IQueryable<TaskEntity>> GetTasksByDateRangeAsync(DateTime from, DateTime to);
    }
}
