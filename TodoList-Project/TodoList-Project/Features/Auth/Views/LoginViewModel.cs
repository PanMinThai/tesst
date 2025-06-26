using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoList_Project.Core.Utils.Exceptions;
using TodoList_Project.Features.Auth.Models;
using TodoList_Project.Features.Auth.Services.Interfaces;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace TodoList_Project.Features.Auth.Views
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string loginMessage;

        public LoginViewModel(IAuthService authService)
        {
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

                // Lưu token hoặc thực hiện điều hướng tại đây
                LoginMessage = $"Chào mừng {result.User.DisplayName}!";

                // Ví dụ: thông báo login thành công
                MessageBox.Show("Đăng nhập thành công!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (UnauthorizedException ex)
            {
                LoginMessage = ex.Message;
            }
            catch (Exception ex)
            {
                LoginMessage = "Có lỗi xảy ra khi đăng nhập.";
                Console.WriteLine(ex);
            }
        }
    }
}