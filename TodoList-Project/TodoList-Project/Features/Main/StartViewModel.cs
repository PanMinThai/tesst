using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Tasks.Services;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Main
{
    public partial class StartViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        private readonly ITaskStatisticsService _taskStatisticsService;
        private readonly ITaskFilterService _taskFilterService;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalTasks))] 
        private int _inProgressCount;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalTasks))]
        private int _completedCount;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalTasks))]
        private int _cancelledCount;

        public int TotalTasks => InProgressCount + CompletedCount + CancelledCount;

        [ObservableProperty]
        private DateTimePeriod _currentFilter = DateTimePeriod.All;

        [ObservableProperty]
        private ObservableCollection<TaskItemViewModel> _tasks = new();
        [ObservableProperty]
        private ObservableCollection<UndoableTaskItemViewModel> _undoableTasks = new();
        [ObservableProperty]
        private int _inProgressCurrentPage = 1;

        [ObservableProperty]
        private int _inProgressTotalPages;

        [ObservableProperty]
        private int _inProgressItemsPerPage = 6; // 2x3 grid
        [ObservableProperty]
        private int _undoableTasksCurrentPage = 1;

        [ObservableProperty]
        private int _undoableTaskTotalPages;

        [ObservableProperty]
        private int _undoableTaskItemsPerPage = 3; // 3x1 grid
        public StartViewModel(ITaskService taskService, ITaskStatisticsService taskStatisticsService,ITaskFilterService taskFilterService)
        {
            _taskService = taskService;
            _taskStatisticsService = taskStatisticsService;
            _taskFilterService = taskFilterService;
            LoadDataCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            await LoadTaskCountsAsync();
            await LoadInProgressTasksAsync();
            await LoadUndoableTasksAsync();
        }

        private async Task LoadTaskCountsAsync()
        {
            try
            {
                var statusCounts = await _taskStatisticsService.GetTaskStatusCountsAsync();

                InProgressCount = statusCounts[TaskStatus.InProgress];
                CompletedCount = statusCounts[TaskStatus.Completed];
                CancelledCount = statusCounts[TaskStatus.Cancelled];
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading task counts: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task InProgressNextPage()
        {
            if (InProgressCurrentPage < InProgressTotalPages)
            {
                InProgressCurrentPage++;
                await LoadInProgressTasksAsync();
            }
        }

        [RelayCommand]
        private async Task InProgressPreviousPage()
        {
            if (InProgressCurrentPage > 1)
            {
                InProgressCurrentPage--;
                await LoadInProgressTasksAsync();
            }
        }
        [RelayCommand]
        private async Task UndoableTasksNextPage()
        {
            if (UndoableTasksCurrentPage < UndoableTaskTotalPages)
            {
                UndoableTasksCurrentPage++;
                await LoadUndoableTasksAsync();
            }
        }

        [RelayCommand]
        private async Task UndoableTasksPreviousPage()
        {
            if (UndoableTasksCurrentPage > 1)
            {
                UndoableTasksCurrentPage--;
                await LoadUndoableTasksAsync();
            }
        }
        private async Task LoadInProgressTasksAsync()
        {
            try
            {
                var (tasks, totalCount) = await _taskFilterService.ApplyFilters(
                    status: TaskStatus.InProgress,
                    priority: null,
                    date: null,
                    pageNumber: InProgressCurrentPage,
                    pageSize: InProgressItemsPerPage);

                InProgressTotalPages = (int)Math.Ceiling((double)totalCount / InProgressItemsPerPage);

                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(new TaskItemViewModel
                    {
                        Title = task.Title,
                        DueDate = task.DueDate?.ToString("MM/dd/yyyy") ?? "No due date",
                        DaysAgo = CalculateTimeDifference(task.DueDate),
                        Priority = task.Priority,
                        Status = task.Status,
                        IsOverdue = task.DueDate.HasValue && task.DueDate.Value.Date < DateTime.Today
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading tasks: {ex.Message}");
            }
        }
        private async Task LoadUndoableTasksAsync()
        {
            try
            {
                var (tasks, totalCount) = await _taskFilterService.GetTodayUpdatedCompletedAndCancelledTasks(
                    pageNumber: UndoableTasksCurrentPage,
                    pageSize: UndoableTaskItemsPerPage);

                UndoableTaskTotalPages = (int)Math.Ceiling((double)totalCount / UndoableTaskItemsPerPage);

                UndoableTasks.Clear();
                foreach (var task in tasks)
                {
                    UndoableTasks.Add(new UndoableTaskItemViewModel
                    {
                        Title = task.Title,
                        DueDate = task.DueDate?.ToString("MM/dd/yyyy") ?? "No due date",
                        DaysAgo = CalculateTimeDifference(task.DueDate),
                        Priority = task.Priority,
                        Status = task.Status,
                        IsOverdue = task.DueDate.HasValue && task.DueDate.Value.Date < DateTime.Today
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading tasks: {ex.Message}");
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
        [RelayCommand]
        private async Task ShowTodayTasksAsync()
        {
            CurrentFilter = DateTimePeriod.Today;
            InProgressCurrentPage = 1;
            await LoadInProgressTasksAsync();
        }

        [RelayCommand]
        private async Task ShowThisWeekTasksAsync()
        {
            CurrentFilter = DateTimePeriod.ThisWeek;
            InProgressCurrentPage = 1;
            await LoadInProgressTasksAsync();
        }

        [RelayCommand]
        private async Task ShowAllTasksAsync()
        {
            CurrentFilter = DateTimePeriod.All;
            InProgressCurrentPage = 1;
            await LoadInProgressTasksAsync();
        }
    }
}
