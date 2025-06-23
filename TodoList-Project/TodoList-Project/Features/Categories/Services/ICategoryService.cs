using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Categories.Models;

namespace TodoList_Project.Features.Categories.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(DateTimePeriod period = DateTimePeriod.All);
        Task UpdateCategoryAsync(CategoryDto model);
    }
}
