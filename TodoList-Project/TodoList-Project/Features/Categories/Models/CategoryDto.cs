using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Categories.Models
{
    public class CategoryDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int TotalTasks;
        public int CompletedTasks;
    }
}
