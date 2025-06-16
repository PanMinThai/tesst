using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.Base;

namespace TodoList_Project.Core.DAL.Entities.SQL
{
    public class CategoryEntity : BaseEntity<int>
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }

        public ICollection<TaskCategoryEntity> TaskCategories { get; set; } = new List<TaskCategoryEntity>();
    }
}
