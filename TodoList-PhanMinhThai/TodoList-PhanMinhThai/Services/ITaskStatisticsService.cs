using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Models;

namespace TodoList_PhanMinhThai.Services
{
    public interface ITaskStatisticsService
    {
        Task<TaskStatistics> GetStatisticsAsync();
    }

}
