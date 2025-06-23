using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoList_Project.Features.Categories.Models
{
    public class TaskCategoryDto
    {
        public int TaskId { get; set; }
        public TaskDto Task { get; set; }
    }
}