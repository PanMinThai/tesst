using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.Utils;
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
        public ObservableCollection<FilterOption<TaskStatus>> StatusFilters { get; }
        public ObservableCollection<FilterOption<TaskPriority>> PriorityFilters { get; }
        #endregion

        #region Filter Properties

        [ObservableProperty]
        private FilterOption<TaskStatus> _selectedStatusFilter;

        [ObservableProperty]
        private FilterOption<TaskPriority> _selectedPriorityFilter;

        partial void OnSelectedStatusFilterChanged(FilterOption<TaskStatus> value)
        {
            _ = ApplyFilters();
        }

        partial void OnSelectedPriorityFilterChanged(FilterOption<TaskPriority> value)
        {
            _ = ApplyFilters();
        }

        [ObservableProperty]
        private DateTime? _selectedDate;

        partial void OnSelectedDateChanged(DateTime? value)
        {
            ApplyDateFilter();
        }

        [ObservableProperty]
        private string _searchKeyword;

        partial void OnSearchKeywordChanged(string value)
        {
            SearchTaskCommand.Execute(null);
        }

        #endregion

        #region State Properties
        [ObservableProperty]
        private TaskModel _selectedTask;

        [ObservableProperty]
        private TaskModel _currentTask = new();

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
        private void ClearTaskFields() => CurrentTask = new TaskModel
        {
            DueDate = DateTime.Today,
            Status = TaskStatus.Completed,
            Priority = TaskPriority.Medium
        };

        [RelayCommand]
        private async Task LoadTasksAsync()
        {
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
        private async Task ApplySearchAsync() => await SearchTaskAsync();

        [RelayCommand]
        private async Task SearchTaskAsync()
        {
            var searched = await _filterService.SearchTasks(
                SearchKeyword,
                SelectedStatusFilter?.Value,
                SelectedPriorityFilter?.Value);

            Tasks.Clear();
            foreach (var task in searched) Tasks.Add(task);
        }
        [RelayCommand]
        private void OpenAddTaskPopup()
        {
            var addTaskViewModel = new AddTaskViewModel(_taskService);
            ShowAddTaskPopup(addTaskViewModel);
        }

       
        #endregion

        #region Constructor
        public ListTaskViewModel(
            ITaskService taskService,
            ITaskFilterService filterService,
            ITaskStatisticsService statisticsService)
        {
            _taskService = taskService;
            _filterService = filterService;
            _statisticsService = statisticsService;

            StatusFilters = new ObservableCollection<FilterOption<TaskStatus>>
            {
                new() { Value = null, DisplayName = "All" },
                new() { Value = TaskStatus.InProgress, DisplayName = "In Progress" },
                new() { Value = TaskStatus.Completed, DisplayName = "Completed" },
                new() { Value = TaskStatus.Cancelled, DisplayName = "Cancelled" },
            };

            PriorityFilters = new ObservableCollection<FilterOption<TaskPriority>>
            {
                new() { Value = null, DisplayName = "All" },
                new() { Value = TaskPriority.High, DisplayName = "High" },
                new() { Value = TaskPriority.Medium, DisplayName = "Medium" },
                new() { Value = TaskPriority.Low, DisplayName = "Low" },
            };

            SelectedStatusFilter = StatusFilters[0];
            SelectedPriorityFilter = PriorityFilters[0];

            LoadTasksCommand.Execute(null);
        }
        #endregion

        #region Private Methods
        private async Task ApplyFilters()
        {
            var filtered = await _filterService.ApplyFilters(
                SelectedStatusFilter?.Value,
                SelectedPriorityFilter?.Value,
                SelectedDate);

            Tasks.Clear();
            foreach (var task in filtered) Tasks.Add(task);
        }

        private async void ApplyDateFilter()
        {
            var tasks = await _filterService.ApplyFilters(
                SelectedStatusFilter?.Value,
                SelectedPriorityFilter?.Value,
                SelectedDate);

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
            var popupView = new PopupView
            {
                DataContext = viewModel,
                Width = 800,
                Height = 450,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };

            var taskControl = new TaskControl
            {
                DataContext = viewModel
            };

            var contentControl = (ContentControl)popupView.FindName("contentControl");
            contentControl.Content = taskControl;

            if (popupView.ShowDialog() == true)
            {
                LoadTasksCommand.Execute(null);
            }
        }
        private void ShowAddTaskPopup(AddTaskViewModel viewModel)
        {
            var popupView = new PopupView
            {
                DataContext = viewModel,
                Width = 800,
                Height = 450,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };

            var addTaskControl = new AddTaskControl
            {
                DataContext = viewModel
            };

            var contentControl = (ContentControl)popupView.FindName("contentControl");
            contentControl.Content = addTaskControl;

            if (popupView.ShowDialog() == true)
            {
                LoadTasksCommand.Execute(null);
            }
        }
        #endregion
    }
}
