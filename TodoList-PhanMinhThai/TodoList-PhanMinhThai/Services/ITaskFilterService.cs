using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Models;

namespace TodoList_PhanMinhThai.Services
{
    public interface ITaskFilterService
    {
        IEnumerable<TaskModel> ApplyFilters(IEnumerable<TaskModel> tasks, Data.Entities.TaskStatus? status, TaskPriority? priority);
    }
}
