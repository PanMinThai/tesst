using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Data.Enums;
using TaskStatus = TodoList_PhanMinhThai.Data.Enums.TaskStatus;


namespace TodoList_PhanMinhThai.Models
{
    public class TaskModel
    { 
        public int Id { get; set; } 
        public string Title {  get; set; }
        public DateTime? DueDate {  get; set; }
        public TaskStatus Status {  get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt {  get; set; }   
        public DateTime UpdatedAt { get; set;}
    }
}
