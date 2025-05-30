using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;
using TodoList_Project.Features.Tasks.Services;

namespace TodoList_Project.Features.Main
{
    public partial class StartViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalTasks))] // Tự động notify khi thay đổi
        private int _inProgressCount;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalTasks))]
        private int _completedCount;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalTasks))]
        private int _cancelledCount;

        public int TotalTasks => InProgressCount + CompletedCount + CancelledCount;

        [ObservableProperty]
        private ObservableCollection<TaskItemViewModel> _tasks = new();

        public StartViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            LoadDataCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadDataAsync()
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
                        TaskStatus.Completed => "#75a7fb",
                        TaskStatus.InProgress => "#7955fd",
                        TaskStatus.Cancelled => "#fb5a9d",
                        _ => "#dcdcdc"
                    }
                });
            }
        }

        private static string CalculateTimeDifference(DateTime? dueDate)
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
