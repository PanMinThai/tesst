using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Tasks.Models;
using TodoList_Project.Features.Tasks.Services;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Views
{
    public partial class TaskViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;

        [ObservableProperty]
        private TaskModel _currentTask = new();

        [ObservableProperty]
        private bool _isEditMode;

        [ObservableProperty]
        private string _selectedCategory;

        public TaskPriority Priority
        {
            get => CurrentTask.Priority;
            set
            {
                if (CurrentTask.Priority != value)
                {
                    CurrentTask.Priority = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsHighPriority));
                    OnPropertyChanged(nameof(IsMediumPriority));
                    OnPropertyChanged(nameof(IsLowPriority));
                }
            }
        }

        public bool IsHighPriority
        {
            get => Priority == TaskPriority.High;
            set { if (value) Priority = TaskPriority.High; }
        }

        public bool IsMediumPriority
        {
            get => Priority == TaskPriority.Medium;
            set { if (value) Priority = TaskPriority.Medium; }
        }

        public bool IsLowPriority
        {
            get => Priority == TaskPriority.Low;
            set { if (value) Priority = TaskPriority.Low; }
        }
        public TaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            InitializeDefaultTask();
        }

        public TaskViewModel(ITaskService taskService, TaskModel taskToEdit) : this(taskService)
        {
            CurrentTask = new TaskModel
            {
                Id = taskToEdit.Id,
                Title = taskToEdit.Title,
                DueDate = taskToEdit.DueDate,
                Status = taskToEdit.Status,
                Priority = taskToEdit.Priority,
                CreatedAt = taskToEdit.CreatedAt
            };
            IsEditMode = true;
        }

        private void InitializeDefaultTask()
        {
            CurrentTask.DueDate = DateTime.Today;
            CurrentTask.Status = TaskStatus.InProgress;
            CurrentTask.Priority = TaskPriority.Medium;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (!ValidateTask())
                return;

            try
            {
                if (IsEditMode)
                {
                    CurrentTask.UpdatedAt = DateTime.Now;
                    await _taskService.UpdateTaskAsync(CurrentTask);
                }
                else
                {
                    CurrentTask.CreatedAt = DateTime.Now;
                    await _taskService.AddTaskAsync(CurrentTask);
                }

                CloseWindow(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (!IsEditMode) return;

            var result = MessageBox.Show("Are you sure you want to delete this task?",
                                      "Confirm Delete",
                                      MessageBoxButton.YesNo,
                                      MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _taskService.DeleteTaskAsync(CurrentTask.Id);
                    CloseWindow(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            CloseWindow(false);
        }

        private bool ValidateTask()
        {
            if (string.IsNullOrWhiteSpace(CurrentTask.Title))
            {
                MessageBox.Show("Please enter a task title", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CurrentTask.DueDate < DateTime.Today && !IsEditMode)
            {
                MessageBox.Show("Due date cannot be in the past", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CloseWindow(bool dialogResult)
        {
            var window = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);

            if (window != null)
            {
                window.DialogResult = dialogResult;
                window.Close();
            }
        }
    }
}
