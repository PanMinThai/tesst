using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Tasks.Models;
using TodoList_Project.Features.Tasks.Services;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Views
{ 

    public partial class ListTaskViewModel : ObservableObject
    {
        #region Services
        private readonly ITaskService _taskService;
        private readonly ITaskFilterService _filterService;
        private readonly ITaskStatisticsService _statisticsService;
        #endregion

        #region Collections
        public ObservableCollection<TaskModel> Tasks { get; } = new();
        public ObservableCollection<TaskStatus?> StatusFilters { get; }
        public ObservableCollection<TaskPriority?> PriorityFilters { get; }
        #endregion

        #region Observable Properties
        [ObservableProperty]
        private TaskModel _selectedTask;

        [ObservableProperty]
        private TaskModel _currentTask = new();

        [ObservableProperty]
        private TaskStatus? _selectedStatus;

        partial void OnSelectedStatusChanged(TaskStatus? value)
        {
            _ = ApplyFilters();
        }

        [ObservableProperty]
        private TaskPriority? _selectedPriority;

        partial void OnSelectedPriorityChanged(TaskPriority? value)
        {
            _ = ApplyFilters();
        }

        [ObservableProperty]
        private string _searchKeyword;

        [ObservableProperty]
        private DateTime? _selectedDate;

        partial void OnSelectedDateChanged(DateTime? value)
        {
            ApplyDateFilter();
        }

        [ObservableProperty]
        private int _yesterdayTaskCount;

        [ObservableProperty]
        private int _todayTaskCount;

        [ObservableProperty]
        private int _thisWeekTaskCount;
        #endregion

        #region Commands
        public ICommand FilterTodayCommand => new RelayCommand(FilterByToday);
        public ICommand FilterYesterdayCommand => new RelayCommand(FilterByYesterday);
        public ICommand FilterThisWeekCommand => new RelayCommand(async () => await FilterByThisWeekAsync());

        [RelayCommand]
        private async Task LoadTasksAsync()
        {
            var allTasks = await _taskService.GetAllTasksAsync();
            await ApplyFilters();

            var stats = await _statisticsService.GetStatisticsAsync();
            YesterdayTaskCount = stats.YesterdayTasksCount;
            TodayTaskCount = stats.TodayTasksCount;
            ThisWeekTaskCount = stats.ThisWeekTasksCount;
        }

        [RelayCommand]
        private async Task AddTaskAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentTask.Title))
            {
                MessageBox.Show("Please enter a task title");
                return;
            }

            await _taskService.AddTaskAsync(CurrentTask);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        [RelayCommand(CanExecute = nameof(CanExecuteSelectedTask))]
        private async Task UpdateTaskAsync()
        {
            CurrentTask.UpdatedAt = DateTime.Now;
            await _taskService.UpdateTaskAsync(CurrentTask);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        [RelayCommand(CanExecute = nameof(CanExecuteSelectedTask))]
        private async Task DeleteTaskAsync()
        {
            await _taskService.DeleteTaskAsync(SelectedTask.Id);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        [RelayCommand(CanExecute = nameof(CanExecuteSelectedTask))]
        private async Task MarkCompleteAsync()
        {
            SelectedTask.Status = TaskStatus.Completed;
            SelectedTask.UpdatedAt = DateTime.Now;
            await _taskService.UpdateTaskAsync(SelectedTask);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        [RelayCommand]
        private async Task ApplySearchAsync()
        {
            var searched = await _filterService.SearchTasks(SearchKeyword, SelectedStatus, SelectedPriority);
            Tasks.Clear();
            foreach (var task in searched) Tasks.Add(task);
        }
        [RelayCommand]
        private async Task SearchTaskAsync()
        {
            var searched = await _filterService.SearchTasks(SearchKeyword, SelectedStatus, SelectedPriority);
            Tasks.Clear();
            foreach (var task in searched) Tasks.Add(task);
        }

        partial void OnSearchKeywordChanged(string value)
        {
            SearchTaskCommand.Execute(null);  
        }
        [RelayCommand]
        private void ClearTaskFields()
        {
            CurrentTask = new TaskModel
            {
                DueDate = DateTime.Today,
                Status = TaskStatus.Completed,
                Priority = TaskPriority.Medium
            };
            SelectedTask = null;
        }
        #endregion

        #region Constructor
        public ListTaskViewModel(ITaskService taskService, ITaskFilterService filterService, ITaskStatisticsService statisticsService)
        {
            _taskService = taskService;
            _filterService = filterService;
            _statisticsService = statisticsService;

            StatusFilters = new ObservableCollection<TaskStatus?>
            {
                null,
                TaskStatus.InProgress,
                TaskStatus.Completed,
                TaskStatus.Cancelled
            };

            PriorityFilters = new ObservableCollection<TaskPriority?>
            {
                null,
                TaskPriority.High,
                TaskPriority.Medium,
                TaskPriority.Low
            };

            LoadTasksCommand.Execute(null);
        }
        #endregion

        #region Private Methods
        private async Task ApplyFilters()
        {
            var filtered = await _filterService.ApplyFilters(SelectedStatus, SelectedPriority, SelectedDate);
            Tasks.Clear();
            foreach (var task in filtered) Tasks.Add(task);
        }

        private async void ApplyDateFilter()
        {
            var tasks = await _filterService.ApplyFilters(SelectedStatus, SelectedPriority, SelectedDate);
            Tasks.Clear();
            foreach (var task in tasks) Tasks.Add(task);
        }

        private void FilterByToday() => SelectedDate = DateTime.Today;

        private void FilterByYesterday() => SelectedDate = DateTime.Today.AddDays(-1);

        private async Task FilterByThisWeekAsync()
        {
            SelectedDate = null;
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            var weekTasks = await _taskService.GetTasksByDateRange(startOfWeek, endOfWeek);
            Tasks.Clear();
            foreach (var task in weekTasks) Tasks.Add(task);
        }

        private bool CanExecuteSelectedTask() => SelectedTask != null;
        #endregion

        [RelayCommand]
        private void OpenAddTaskPopup()
        {
            var taskViewModel = new TaskViewModel(_taskService);
            ShowTaskPopup(taskViewModel);
        }

        partial void OnSelectedTaskChanged(TaskModel value)
        {
            if (value != null)
            {
                var taskViewModel = new TaskViewModel(_taskService, value);
                ShowTaskPopup(taskViewModel);
            }
        }

        private void ShowTaskPopup(TaskViewModel viewModel)
        {
            var popupView = new PopupView();

            // Đặt DataContext cho PopupView
            popupView.DataContext = viewModel;

            var taskControl = new TaskControl();
            taskControl.DataContext = viewModel;

            var contentControl = (ContentControl)popupView.FindName("contentControl");
            contentControl.Content = taskControl;

            popupView.Width = 800;
            popupView.Height = 450;
            popupView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            popupView.Owner = Application.Current.MainWindow;

            if (popupView.ShowDialog() == true)
            {
                LoadTasksCommand.Execute(null);
            }
        }
    }
}
