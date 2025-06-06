using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Windows;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.Utils.Converters;
using TodoList_Project.Core.Utils.Messages;
using TodoList_Project.Features.Tasks.Models;
using TodoList_Project.Features.Tasks.Services;
using TodoList_Project.Features.Tasks.Views;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Views
{
    public partial class AddTaskViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;

        // Properties for binding
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private DateTime? _dueDate;

        [ObservableProperty]
        private double _priorityLevel = 3;

        [ObservableProperty]
        private TaskModel _newTask = new();

        public AddTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            InitializeDefaultValues();
        }

        private void InitializeDefaultValues()
        {
            DueDate = DateTime.Today;
            NewTask.Priority = TaskPriority.Medium;
            NewTask.Status = TaskStatus.InProgress;
            NewTask.CreatedAt = DateTime.Now;
            NewTask.UpdatedAt = DateTime.Now;
        }

        [RelayCommand]
        private async Task AddTask()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Please enter a task title", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Map view properties to the task model
                NewTask.Title = Title;
                NewTask.DueDate = DueDate;
                NewTask.Priority = ConvertRatingToPriority(PriorityLevel);
                NewTask.UpdatedAt = DateTime.Now;

                await _taskService.AddTaskAsync(NewTask);
                WeakReferenceMessenger.Default.Send(new TaskAddedMessage(NewTask));
                ClosePopup();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Close()
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            Application.Current.Windows.OfType<PopupView>().FirstOrDefault()?.Close();
        }

        private TaskPriority ConvertRatingToPriority(double rating)
        {
            return rating switch
            {
                <= 1 => TaskPriority.Low,
                <= 2 => TaskPriority.Low,
                <= 3 => TaskPriority.Medium,
                <= 4 => TaskPriority.High,
                _ => TaskPriority.High,
            };
        }
    }
}