using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities.Base;

namespace TodoList_PhanMinhThai.Data.Entities
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

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<TaskCategoryEntity> TaskCategories { get; set; } = new List<TaskCategoryEntity>();
    }

    public enum TaskStatus
    {
        [Display(Name = "Đang làm")]
        InProgress,

        [Display(Name = "Hoàn thành")]
        Completed,
        [Display(Name = "Hủy")]
        Cancelled
    }

    public enum TaskPriority
    {
        [Display(Name = "Cao")]
        High,

        [Display(Name = "Trung bình")]
        Medium,

        [Display(Name = "Thấp")]
        Low
    }
}
