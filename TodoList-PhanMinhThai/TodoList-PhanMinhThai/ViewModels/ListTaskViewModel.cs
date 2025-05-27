using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Models;
using TodoList_PhanMinhThai.Repositories;
using TodoList_PhanMinhThai.Utilities;
using TaskStatus = TodoList_PhanMinhThai.Data.Entities.TaskStatus;

namespace TodoList_PhanMinhThai.ViewModels
{
    public class ListTaskViewModel : ViewModelBase
    {
        public int YesterdayTaskCount { get; private set; }
        public int TodayTaskCount { get; private set; }
        public int ThisWeekTaskCount { get; private set; }

        private readonly ITaskRepository _taskRepository;
        private TaskModel _selectedTask;
        private TaskModel _currentTask = new TaskModel();

        public ObservableCollection<TaskModel> Tasks { get; } = new ObservableCollection<TaskModel>();
        public ObservableCollection<TaskStatus> StatusOptions { get; } = new ObservableCollection<TaskStatus>(Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>());

        public ObservableCollection<TaskPriority> PriorityOptions { get; } = new ObservableCollection<TaskPriority>(Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>());

        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                if (_selectedTask != null)
                {
                    CurrentTask = new TaskModel
                    {
                        Id = _selectedTask.Id,
                        Title = _selectedTask.Title,
                        DueDate = _selectedTask.DueDate,
                        Status = _selectedTask.Status,
                        Priority = _selectedTask.Priority
                    };
                }
                OnPropertyChanged(nameof(SelectedTask));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public TaskModel CurrentTask
        {
            get => _currentTask;
            set
            {
                _currentTask = value;
                OnPropertyChanged(nameof(CurrentTask));
            }
        }

        public ICommand LoadTasksCommand { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand ClearTaskCommand { get; }
        public ICommand MarkCompleteCommand { get; }
        public ICommand SearchTaskCommand { get; }
            
        public ListTaskViewModel(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

            // Khởi tạo commands
            LoadTasksCommand = new RelayCommand(async _ => await LoadTasksAsync());
            AddTaskCommand = new RelayCommand(async _ => await AddTaskAsync());
            UpdateTaskCommand = new RelayCommand(async _ => await UpdateTaskAsync(), _ => CanExecuteTaskCommand());
            DeleteTaskCommand = new RelayCommand(async _ => await DeleteTaskAsync(), _ => CanExecuteTaskCommand());
            ClearTaskCommand = new RelayCommand(_ => ClearTaskFields());
            MarkCompleteCommand = new RelayCommand(async _ => await MarkTaskCompleteAsync(), _ => CanExecuteTaskCommand());
          //  SearchTaskCommand = new RelayCommand(async _ => await SearchTaskAsync());
            // Tải tasks khi khởi tạo
            LoadTaskCounts();
            LoadTasksCommand.Execute(null);
        }

        private async Task LoadTasksAsync()
        {
            Tasks.Clear();
            var tasks = await _taskRepository.GetAllTasksAsync();
            foreach (var task in tasks.OrderBy(t => t.DueDate))
            {
                Tasks.Add(task);
            }
        }
        private void LoadTaskCounts()
        {
            YesterdayTaskCount = _taskRepository.GetYesterdayTaskCount();
            TodayTaskCount = _taskRepository.GetTodayTaskCount();
            ThisWeekTaskCount = _taskRepository.GetThisWeekTaskCount();

            OnPropertyChanged(nameof(YesterdayTaskCount));
            OnPropertyChanged(nameof(TodayTaskCount));
            OnPropertyChanged(nameof(ThisWeekTaskCount));
        }

        private async Task AddTaskAsync()
        {

            try
            {
                if (string.IsNullOrWhiteSpace(CurrentTask.Title))
                {
                    MessageBox.Show("Please enter a task title");
                    return;
                }

                await _taskRepository.AddTaskAsync(CurrentTask);
                await LoadTasksAsync();
                ClearTaskFields();
            }
            catch (RepositoryException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
                // Log error (ex.InnerException) nếu cần
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
        }

        private async Task UpdateTaskAsync()
        {
            CurrentTask.UpdatedAt = DateTime.Now;
            await _taskRepository.UpdateTaskAsync(CurrentTask);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        private async Task DeleteTaskAsync()
        {
            await _taskRepository.DeleteTaskAsync(SelectedTask.Id);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        private async Task MarkTaskCompleteAsync()
        {
            SelectedTask.Status = Data.Entities.TaskStatus.Completed;
            SelectedTask.UpdatedAt = DateTime.Now;
            await _taskRepository.UpdateTaskAsync(SelectedTask);
            await LoadTasksAsync();
            ClearTaskFields();
        }

        private void ClearTaskFields()
        {
            CurrentTask = new TaskModel
            {
                DueDate = DateTime.Today,
                Status = Data.Entities.TaskStatus.Completed,
                Priority = Data.Entities.TaskPriority.Medium
            };
            SelectedTask = null;
        }

        private bool CanExecuteTaskCommand()
        {
            return SelectedTask != null;
        }
    }
}
