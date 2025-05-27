using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using TaskStatus = TodoList_PhanMinhThai.Data.Entities.TaskStatus;

namespace TodoList_PhanMinhThai.ViewModels
{
    class TaskItemViewModel :ViewModelBase
    {
        // Properties
        private string _background = "#FFFFFF";
        public string Background
        {
            get => _background;
            set
            {
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }
        private string _title = "UX Design Project";
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _dueDate = "December 23, 2018";
        public string DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                OnPropertyChanged(nameof(DueDate));
            }
        }

        private string _daysAgo = "5 Days Ago";
        public string DaysAgo
        {
            get => _daysAgo;
            set
            {
                _daysAgo = value;
                OnPropertyChanged(nameof(DaysAgo));
            }
        }

        private TaskPriority _priority = TaskPriority.High;
        public TaskPriority Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        private TaskStatus _status = TaskStatus.Completed;
        public TaskStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

    }
}
