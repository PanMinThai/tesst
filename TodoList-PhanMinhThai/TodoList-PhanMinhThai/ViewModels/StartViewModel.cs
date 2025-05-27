using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Repositories;

namespace TodoList_PhanMinhThai.ViewModels
{
    class StartViewModel : ViewModelBase
    {
        private readonly ITaskRepository _taskRepository;
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
        public StartViewModel(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            Tasks = new ObservableCollection<TaskItemViewModel>();
            LoadTaskCounts();
            LoadTasks();
        }

        private void LoadTaskCounts()
        {
            InProgressCount = _taskRepository.GetInProgressCount();
            CompletedCount = _taskRepository.GetCompletedCount();
            CancelledCount = _taskRepository.GetCancelledCount();

            OnPropertyChanged(nameof(InProgressCount));
            OnPropertyChanged(nameof(CompletedCount));
            OnPropertyChanged(nameof(CancelledCount));
        }
        private async Task LoadTasks()
        {
            var tasksFromDb = await _taskRepository.GetAllTasksAsync(); 

            Tasks.Clear();

            foreach (var task in tasksFromDb)
            {
                Tasks.Add(new TaskItemViewModel
                {
                    Title = task.Title,
                    DueDate = task.DueDate?.ToString("MMMM dd, yyyy") ?? "No due date", // Nếu null sẽ hiển thị "No due date" 
                    DaysAgo = CalculateTimeDifference(task.DueDate), // Tính toán số ngày trước
                    Priority = task.Priority,
                    Status = task.Status,
                    Background = task.Status switch
                    {
                        Data.Entities.TaskStatus.Completed => "#75a7fb",
                        Data.Entities.TaskStatus.InProgress => "#7955fd",
                        Data.Entities.TaskStatus.Cancelled => "#fb5a9d"
                    }
                });
            }
            var t = Tasks;
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
