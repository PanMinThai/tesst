using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Categories.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; }
    }
}
