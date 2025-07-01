using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using TodoList_Project.Core.DAL.Repositories.Interfaces;
using TodoList_Project.Core.Utils.Exceptions;
using TodoList_Project.Features.Auth.Models;
using TodoList_Project.Features.Auth.Services.Interfaces;
using TodoList_Project.Features.Categories.Services;
using TodoList_Project.Features.Main;
using TodoList_Project.Features.Main.Services;
using TodoList_Project.Features.Tasks.Services;

namespace TodoList_Project.Features.Auth.Views
{
    public partial class LoginViewModel : ObservableObject
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
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string loginMessage;

        public LoginViewModel(
            IAuthService authService,
            ITaskRepository taskRepository,
            ITaskService taskService,
            ITaskStatisticsService taskStatisticsService,
            ITaskFilterService taskFilterService,
            ICategoryService categoryService,
            IMessenger messenger,
            IFeedbackService feedbackService)
        {
            _taskRepository = taskRepository;
            _taskService = taskService;
            _taskStatisticsService = taskStatisticsService;
            _taskFilterService = taskFilterService;
            _categoryService = categoryService;
            _feedbackService = feedbackService;
            _messenger = messenger;
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                var loginModel = new LoginModel
                {
                    Email = Email,
                    Password = Password
                };

                var result = await _authService.LoginAsync(loginModel);

                // Lưu session vào ứng dụng
                App.Current.Properties["SessionToken"] = result.Session.Token;
                App.Current.Properties["UserId"] = result.User.Id;

                LoginMessage = $"Chào mừng {result.User.DisplayName}!";

                var mainViewModel = new MainViewModel(
                    _taskRepository,
                    _taskService,
                    _taskStatisticsService,
                    _taskFilterService,
                    _categoryService,
                    _messenger,
                    _feedbackService);

                // Ví dụ: thông báo login thành công
                MessageBox.Show("Đăng nhập thành công!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (UnauthorizedException ex)
            {
                LoginMessage = ex.Message;
                MessageBox.Show(ex.Message, "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (AccountLockedException ex)
            {
                LoginMessage = ex.Message;
                MessageBox.Show(ex.Message, "Tài khoản bị khóa", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                LoginMessage = "Có lỗi xảy ra khi đăng nhập.";
                Console.WriteLine(ex);
            }
        }
    }
}