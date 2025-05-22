using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TodoList_PhanMinhThai.Models;
using TodoList_PhanMinhThai.Repositories;
using TodoList_PhanMinhThai.Utilities;

namespace TodoList_PhanMinhThai.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private readonly ITaskRepository _taskRepository;
        private TaskModel _selectedTask;
        private TaskModel _currentTask = new TaskModel();

        public ObservableCollection<TaskModel> Tasks { get; } = new ObservableCollection<TaskModel>();
        public ObservableCollection<string> StatusOptions { get; } = new ObservableCollection<string> { "Đang làm", "Hoàn thành" };
        public ObservableCollection<string> PriorityOptions { get; } = new ObservableCollection<string> { "Cao", "Trung bình", "Thấp" };

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
                        Description = _selectedTask.Description,
                        DueDate = _selectedTask.DueDate,
                        Status = _selectedTask.Status,
                        Priority = _selectedTask.Priority
                    };
                }
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public TaskModel CurrentTask
        {
            get => _currentTask;
            set
            {
                _currentTask = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadTasksCommand { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand ClearTaskCommand { get; }
        public ICommand MarkCompleteCommand { get; }

        public TaskViewModel(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

            // Khởi tạo commands
            LoadTasksCommand = new RelayCommand(async _ => await LoadTasksAsync());
            AddTaskCommand = new RelayCommand(async _ => await AddTaskAsync());
            UpdateTaskCommand = new RelayCommand(async _ => await UpdateTaskAsync(), _ => CanExecuteTaskCommand());
            DeleteTaskCommand = new RelayCommand(async _ => await DeleteTaskAsync(), _ => CanExecuteTaskCommand());
            ClearTaskCommand = new RelayCommand(_ => ClearTaskFields());
            MarkCompleteCommand = new RelayCommand(async _ => await MarkTaskCompleteAsync(), _ => CanExecuteTaskCommand());

            // Tải tasks khi khởi tạo
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}