using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Enums;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Main
{
    public partial class TaskItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _background = "#FFFFFF";

        [ObservableProperty]
        private string _title = "UX Design Project";

        [ObservableProperty]
        private string _dueDate = "December 23, 2018";

        [ObservableProperty]
        private string _daysAgo = "5 Days Ago";

        [ObservableProperty]
        private TaskPriority _priority = TaskPriority.High;

        [ObservableProperty]
        private TaskStatus _status = TaskStatus.Completed;
    }
}
