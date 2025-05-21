using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_PhanMinhThai.Models
{
    public class TaskModel
    { 
        public int Id { get; set; } 
        public string Title {  get; set; }
        public string Description { get; set; }
        public DateTime DueDate {  get; set; }
        public string Status {  get; set; }
        public string Priority { get; set; }
        public DateTime CreatAt {  get; set; }   
        public DateTime UpdateAt { get; set;}
    }
}
