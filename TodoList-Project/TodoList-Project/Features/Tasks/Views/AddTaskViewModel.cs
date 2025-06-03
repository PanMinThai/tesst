using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoList_Project.Features.Tasks.Models;
using TodoList_Project.Features.Tasks.Services;

namespace TodoList_Project.Features.Tasks.Views
{
    public partial class AddTaskViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;

        [ObservableProperty]
        private TaskModel _newTask = new();

        public AddTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [RelayCommand]
        private async Task AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTask.Title))
            {
                MessageBox.Show("Please enter a task title");
                return;
            }

            await _taskService.AddTaskAsync(NewTask);
            Application.Current.Windows.OfType<PopupView>().FirstOrDefault()?.Close();
        }
    }
}
