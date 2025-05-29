using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Models;
using TodoList_PhanMinhThai.Services;
using TodoList_PhanMinhThai.Utilities;
using TaskStatus = TodoList_PhanMinhThai.Data.Entities.TaskStatus;

namespace TodoList_PhanMinhThai.ViewModels
{
    public class ListTaskViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskFilterService _filterService;
        private readonly ITaskStatisticsService _statisticsService;

        public ObservableCollection<TaskModel> Tasks { get; } = new();
        public ObservableCollection<TaskModel> AllTasks { get; } = new();

        public ObservableCollection<TaskStatus?> StatusFilters { get; }
        public ObservableCollection<TaskPriority?> PriorityFilters { get; }

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                if (value != null)
                {
                    CurrentTask = new TaskModel
                    {
                        Id = value.Id,
                        Title = value.Title,
                        DueDate = value.DueDate,
                        Status = value.Status,
                        Priority = value.Priority
                    };
                }
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        private TaskModel _currentTask = new();
        public TaskModel CurrentTask
        {
            get => _currentTask;
            set { 
                _currentTask = value; 
                OnPropertyChanged(nameof(CurrentTask)); 
            }
        }

        private TaskStatus? _selectedStatus;
        public TaskStatus? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                ApplyFilters();
            }
        }

        private TaskPriority? _selectedPriority;
        public TaskPriority? SelectedPriority
        {
            get => _selectedPriority;
            set
            {
                _selectedPriority = value;

                OnPropertyChanged(nameof(SelectedPriority));
                ApplyFilters();
            }
        }
        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged(nameof(SearchKeyword));
            }
        }
        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                ApplyDateFilter(); // mỗi lần chọn ngày sẽ lọc
            }
        }


        public int YesterdayTaskCount { get; private set; }
        public int TodayTaskCount { get; private set; }
        public int ThisWeekTaskCount { get; private set; }

        public ICommand FilterTodayCommand { get; }
        public ICommand FilterYesterdayCommand { get; }
        public ICommand FilterThisWeekCommand { get; }
        public ICommand LoadTasksCommand { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand SearchTaskCommand { get; }
        public ICommand ClearTaskCommand { get; }
        public ICommand MarkCompleteCommand { get; }
        public ListTaskViewModel( ITaskService taskService, ITaskFilterService filterService, ITaskStatisticsService statisticsService)
        {
            _taskService = taskService;
            _filterService = filterService;
            _statisticsService = statisticsService;

            StatusFilters = new ObservableCollection<TaskStatus?> { null, TaskStatus.InProgress, TaskStatus.Completed, TaskStatus.Cancelled };
            PriorityFilters = new ObservableCollection<TaskPriority?> { null, TaskPriority.High, TaskPriority.Medium, TaskPriority.Low };

            LoadTasksCommand = new RelayCommand(async _ => await LoadTasksAsync());
            AddTaskCommand = new RelayCommand(async _ => await AddTaskAsync());
            UpdateTaskCommand = new RelayCommand(async _ => await UpdateTaskAsync(), _ => SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(async _ => await DeleteTaskAsync(), _ => SelectedTask != null);
            ClearTaskCommand = new RelayCommand(_ => ClearTaskFields());
            MarkCompleteCommand = new RelayCommand(async _ => await MarkTaskCompleteAsync(), _ => SelectedTask != null);
            SearchTaskCommand = new RelayCommand(_ => ApplySearch());
            FilterTodayCommand = new RelayCommand(_ => FilterByToday());
            FilterYesterdayCommand = new RelayCommand(_ => FilterByYesterday());
            FilterThisWeekCommand = new RelayCommand(_ => FilterByThisWeek());

            LoadTasksCommand.Execute(null);
        }

        private async Task LoadTasksAsync()
        {
            var allTasks = await _taskService.GetAllTasksAsync();
            AllTasks.Clear();
            foreach (var task in allTasks) AllTasks.Add(task);
            ApplyFilters();

            var stats = await _statisticsService.GetStatisticsAsync();
            YesterdayTaskCount = stats.YesterdayTasksCount;
            TodayTaskCount = stats.TodayTasksCount;
            ThisWeekTaskCount = stats.ThisWeekTasksCount;

            OnPropertyChanged(nameof(YesterdayTaskCount));
            OnPropertyChanged(nameof(TodayTaskCount));
            OnPropertyChanged(nameof(ThisWeekTaskCount));
        }

        private async Task ApplyFilters()
        {
            var filtered = await _filterService.ApplyFilters(SelectedStatus, SelectedPriority);
            Tasks.Clear();
            foreach (var task in filtered) Tasks.Add(task);
        }
        private void ApplySearch()
        {
            var searched = await _taskService.SearchTasksAsync(SearchKeyword, SelectedStatus, SelectedPriority);

            Tasks.Clear();
            foreach (var task in searched) Tasks.Add(task);
        }
        private void ApplyDateFilter()
        {
            if (SelectedDate.HasValue)
            {
                var filtered = AllTasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == SelectedDate.Value.Date);
                Tasks.Clear();
                foreach (var task in filtered) Tasks.Add(task);
            }
        }
            
        private void FilterByToday()
        {
            SelectedDate = DateTime.Today;
        }

        private void FilterByYesterday()
        {
            SelectedDate = DateTime.Today.AddDays(-1);
        }

        private void FilterByThisWeek()
        {
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(7);

            var filtered = AllTasks.Where(t =>
                t.DueDate?.Date >= startOfWeek &&
                t.DueDate?.Date < endOfWeek);

            Tasks.Clear();
            foreach (var task in filtered) Tasks.Add(task);
        }

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

        private async Task UpdateTaskAsync()
        {
            CurrentTask.UpdatedAt = DateTime.Now;
            await _taskService.UpdateTaskAsync(CurrentTask);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        private async Task DeleteTaskAsync()
        {
            await _taskService.DeleteTaskAsync(SelectedTask.Id);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        private async Task MarkTaskCompleteAsync()
        {
            SelectedTask.Status = TaskStatus.Completed;
            SelectedTask.UpdatedAt = DateTime.Now;
            await _taskService.UpdateTaskAsync(SelectedTask);
            await LoadTasksAsync();
            ClearTaskFields();
        }

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
    }

}