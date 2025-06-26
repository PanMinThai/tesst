using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TodoList_Project.Features.Auth.Services.Interfaces;

namespace TodoList_Project.Features.Auth.Views
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        [ObservableProperty]
        private ObservableObject _currentControl;
        public AuthViewModel(IAuthService authService)
        {
            _authService = authService;
            CurrentControl = new LoginViewModel(_authService);

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
            CurrentControl = new LoginViewModel(_authService);
        }
        #endregion
    }
}
