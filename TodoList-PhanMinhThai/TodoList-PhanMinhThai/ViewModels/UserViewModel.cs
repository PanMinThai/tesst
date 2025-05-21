using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList_PhanMinhThai.Models;
using TodoList_PhanMinhThai.Utilities;

namespace TodoList_PhanMinhThai.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private UserModel _selectedUser;
        private string _username;
        private string _password;
        private string _email;

        public ObservableCollection<UserModel> Users { get; } = new ObservableCollection<UserModel>();

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    Username = _selectedUser.Username;
                    Password = _selectedUser.Password;
                    Email = _selectedUser.Email;
                }
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public UserViewModel()
        {
            // Khởi tạo commands
            AddCommand = new RelayCommand(AddUser);
            UpdateCommand = new RelayCommand(UpdateUser, CanExecuteUpdateOrDelete);
            DeleteCommand = new RelayCommand(DeleteUser, CanExecuteUpdateOrDelete);
            ClearCommand = new RelayCommand(ClearFields);

            // Thêm vài user mẫu
            Users.Add(new UserModel { Username = "admin", Password = "admin123", Email = "admin@example.com" });
            Users.Add(new UserModel { Username = "user1", Password = "user1123", Email = "user1@example.com" });
        }

        private void AddUser(object parameter)
        {
            var newUser = new UserModel
            {
                Username = Username,
                Password = Password,
                Email = Email
            };
            Users.Add(newUser);
            ClearFields(null);
        }

        private void UpdateUser(object parameter)
        {
            SelectedUser.Username = Username;
            SelectedUser.Password = Password;
            SelectedUser.Email = Email;

            // Cập nhật UI (do ObservableCollection không tự phát hiện thay đổi thuộc tính)
            var index = Users.IndexOf(SelectedUser);
            Users.RemoveAt(index);
            Users.Insert(index, SelectedUser);

            ClearFields(null);
        }

        private void DeleteUser(object parameter)
        {
            Users.Remove(SelectedUser);
            ClearFields(null);
        }

        private void ClearFields(object parameter)
        {
            SelectedUser = null;
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
        }

        private bool CanExecuteUpdateOrDelete(object parameter)
        {
            return SelectedUser != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
