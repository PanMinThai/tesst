using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Tasks.Models;

namespace TodoList_Project.Core.Utils.Messages
{
    public class TaskUpdatedMessage : MessageGeneric<TaskModel>
    {
        public TaskUpdatedMessage(TaskModel task) : base(task) { }
    }
}
