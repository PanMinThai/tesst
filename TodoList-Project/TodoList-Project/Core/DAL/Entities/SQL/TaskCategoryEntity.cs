using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.Base;

namespace TodoList_Project.Core.DAL.Entities.SQL
{
    public class TaskCategoryEntity
    {
        public int TaskId { get; set; }
        public TaskEntity Task { get; set; }

        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
