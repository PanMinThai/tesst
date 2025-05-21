using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities.Base;

namespace TodoList_PhanMinhThai.Data.Entities
{
    public class CategoryEntity : BaseEntity<int>
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<TaskCategoryEntity> TaskCategories { get; set; } = new List<TaskCategoryEntity>();
    }
}
