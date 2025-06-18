using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.Utils.Messages;
using TodoList_Project.Features.Tasks.Models;
using TodoList_Project.Features.Tasks.Services;
using static System.Net.Mime.MediaTypeNames;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Main
{
    public partial class TaskItemViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        [ObservableProperty]
        private int _id;
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

        [ObservableProperty]
        private bool _isOverdue;
        [ObservableProperty]
        private bool _isCompleted;

        // Colors
        public SolidColorBrush BackgroundColor => IsOverdue
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fef2f2"))
            : new SolidColorBrush(Colors.White);

        public SolidColorBrush BorderColor => IsOverdue
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f87373"))
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5e7eb"));

        public SolidColorBrush TimeTextColor => DaysAgo switch
        {
            "Today" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981")),
            "Tomorrow" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6")),
            "Yesterday" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444")),
            _ => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B7280"))
        };

        public SolidColorBrush PriorityBackground => Priority switch
        {
            TaskPriority.High => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEE2E2")),
            TaskPriority.Medium => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF3C7")),
            TaskPriority.Low => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCFCE7")),
            _ => new SolidColorBrush(Colors.Transparent)
        };

        public SolidColorBrush PriorityTextColor => Priority switch
        {
            TaskPriority.High => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626")),
            TaskPriority.Medium => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92400E")),
            TaskPriority.Low => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#166534")),
            _ => new SolidColorBrush(Colors.Black)
        };
        public TaskItemViewModel(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [RelayCommand]
        private async Task CancelTaskAsync()
        {
            var taskModel = new TaskModel
            {
                Id = this.Id,
                Title = this.Title,
                DueDate = DateTime.Parse(this.DueDate),
                Priority = this.Priority,
                Status = TaskStatus.Cancelled,
            };

            await _taskService.UpdateTaskAsync(taskModel);
            WeakReferenceMessenger.Default.Send(new TaskUpdatedMessage(taskModel));
            WeakReferenceMessenger.Default.Send(
                new ShowCharacterMessage("Anya khinh bỉ những đứa hủy task. Hèn quá")
            );
        }
        partial void OnIsCompletedChanged(bool value)
        {
            if (value)
            {
                _ = MarkTaskAsCompletedAsync();
            }
        }
        private async Task MarkTaskAsCompletedAsync()
        {
            var taskModel = new TaskModel
            {
                Id = this.Id,
                Title = this.Title,
                DueDate = DateTime.Parse(this.DueDate),
                Priority = this.Priority,
                Status = TaskStatus.Completed,
            };

            await _taskService.UpdateTaskAsync(taskModel);
            WeakReferenceMessenger.Default.Send(new TaskUpdatedMessage(taskModel));
        }
    }
}
