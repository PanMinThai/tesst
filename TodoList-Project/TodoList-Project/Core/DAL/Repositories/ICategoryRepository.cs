using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL;

namespace TodoList_Project.Core.DAL.Repositories
{
    public interface ICategoryRepository : IGenericRepository<CategoryEntity>
    {
        Task<IEnumerable<CategoryEntity>> GetAllWithTaskStatsAsync();
    }
}
