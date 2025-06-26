using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.Base;
using TodoList_Project.Core.DAL.Enums;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;

namespace TodoList_Project.Core.DAL.Entities.SQL
{
    public class TaskEntity : BaseEntity<int>
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.InProgress;

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        [ForeignKey(nameof(AssignedUser))]
        public Guid AssignedUserId { get; set; }  

        // Navigation properties
        public UserEntity AssignedUser { get; set; } 
        public ICollection<TaskCategoryEntity> TaskCategories { get; set; } = new List<TaskCategoryEntity>();
    }
}
