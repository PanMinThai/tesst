using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public interface ITaskRepository : IGenericRepository<TaskEntity, int>, ITaskStatisticsRepository
    {
        Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetFilteredTasksAsync(TaskStatus? status = null, TaskPriority? priority = null, string keyword = null, DateTime? date = null, DateTime? fromDate = null, DateTime? toDate = null, int pageNumber = 1, int pageSize = 10);
        Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetTodayUpdatedCompletedAndCancelledTasks(int pageNumber = 1, int pageSize = 10);
    }
}
