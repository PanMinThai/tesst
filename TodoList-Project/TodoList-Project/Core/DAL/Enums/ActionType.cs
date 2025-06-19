using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Enums
{
    public enum ActionType
    {
        Cancelled,
        Completed,
        UndoCancelled,
        UndoCompleted,
        Create,
        Update,
        Delete,
        Notification
    }

}
