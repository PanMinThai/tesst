using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.Enums;

namespace TodoList_Project.Features.Categories.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public Core.DAL.Enums.TaskStatus Status { get; set; }
    }
}
