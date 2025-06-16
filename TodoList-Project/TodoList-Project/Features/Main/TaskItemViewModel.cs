using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private string _background;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _dueDate;

        [ObservableProperty]
        private string _daysAgo;

        [ObservableProperty]
        private TaskPriority _priority;

        [ObservableProperty]
        private TaskStatus _status;
       
    }
}
