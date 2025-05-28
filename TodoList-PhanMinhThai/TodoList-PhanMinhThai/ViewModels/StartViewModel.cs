using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Repositories;
using TodoList_PhanMinhThai.Services;
//TODO: sử dụng MVVMToolkit
namespace TodoList_PhanMinhThai.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;

        public int InProgressCount { get; private set; }
        public int CompletedCount { get; private set; }
        public int CancelledCount { get; private set; }

        private ObservableCollection<TaskItemViewModel> _tasks;
        public ObservableCollection<TaskItemViewModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public StartViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<TaskItemViewModel>();

            // Gọi async từ constructor thông qua Task.Run hoặc dùng async void
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            await LoadTaskCountsAsync();
            await LoadTasksAsync();
        }

        private async Task LoadTaskCountsAsync()
        {
            var statistics = await _taskService.GetTaskStatisticsAsync();

            InProgressCount = statistics.InProgressCount;
            CompletedCount = statistics.CompletedCount;
            CancelledCount = statistics.CancelledCount;

            OnPropertyChanged(nameof(InProgressCount));
            OnPropertyChanged(nameof(CompletedCount));
            OnPropertyChanged(nameof(CancelledCount));
        }

        private async Task LoadTasksAsync()
        {
            var taskModels = await _taskService.GetAllTasksAsync();

            Tasks.Clear();

            foreach (var task in taskModels)
            {
                Tasks.Add(new TaskItemViewModel
                {
                    Title = task.Title,
                    DueDate = task.DueDate?.ToString("MMMM dd, yyyy") ?? "No due date",
                    DaysAgo = CalculateTimeDifference(task.DueDate),
                    Priority = task.Priority,
                    Status = task.Status,
                    Background = task.Status switch
                    {
                        Data.Entities.TaskStatus.Completed => "#75a7fb",
                        Data.Entities.TaskStatus.InProgress => "#7955fd",
                        Data.Entities.TaskStatus.Cancelled => "#fb5a9d",
                        _ => "#dcdcdc"
                    }
                });
            }
        }

        private string CalculateTimeDifference(DateTime? dueDate)
        {
            if (!dueDate.HasValue) return "No due date";

            var difference = dueDate.Value.Date - DateTime.Today;
            int days = difference.Days;

            return days switch
            {
                0 => "Today",
                1 => "Tomorrow",
                > 1 => $"in {days} days",
                -1 => "Yesterday",
                < -1 => $"{Math.Abs(days)} days ago"
            };
        }
    }

}
