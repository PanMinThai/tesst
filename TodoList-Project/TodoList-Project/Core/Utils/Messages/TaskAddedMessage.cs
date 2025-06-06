using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Tasks.Models;

namespace TodoList_Project.Core.Utils.Messages
{
    /// <summary>
    /// Message sent when a new task is added.
    /// </summary>
    public class TaskAddedMessage : MessageGeneric<TaskModel>
    {
        public TaskAddedMessage(TaskModel task) : base(task)
        {
        }
    }
}
