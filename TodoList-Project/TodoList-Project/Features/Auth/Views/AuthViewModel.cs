using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TodoList_Project.Core.DAL.Repositories.Interfaces;
using TodoList_Project.Features.Auth.Services;
using TodoList_Project.Features.Auth.Services.Interfaces;
using TodoList_Project.Features.Categories.Services;
using TodoList_Project.Features.Main.Services;
using TodoList_Project.Features.Tasks.Services;

namespace TodoList_Project.Features.Auth.Views
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskService _taskService;
        private readonly ITaskFilterService _taskFilterService;
        private readonly ITaskStatisticsService _taskStatisticsService;
        private readonly ICategoryService _categoryService;
        private readonly IFeedbackService _feedbackService;
        private readonly IMessenger _messenger;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ObservableObject _currentControl;
        public AuthViewModel(IAuthService authService)
        {
            _authService = authService;
            CurrentControl = new LoginViewModel(_authService, _taskRepository, _taskService, _taskStatisticsService, _taskFilterService, _categoryService, _messenger, _feedbackService);

            // Initialize commands
            CloseWindowCommand = new RelayCommand(CloseWindow);
            SwitchToRegisterCommand = new RelayCommand(SwitchToRegister);
            SwitchToLoginCommand = new RelayCommand(SwitchToLogin);
        }

        #region Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand SwitchToRegisterCommand { get; }
        public ICommand SwitchToLoginCommand { get; }
        #endregion
        #region 

        private void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        private void SwitchToRegister()
        {
            //CurrentControl = new RegisterViewModel();
        }

        private void SwitchToLogin()
        {
            CurrentControl = new LoginViewModel(_authService, _taskRepository, _taskService, _taskStatisticsService, _taskFilterService, _categoryService, _messenger, _feedbackService);
        }
        #endregion
    }
}
