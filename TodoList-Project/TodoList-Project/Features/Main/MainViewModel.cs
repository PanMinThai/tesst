using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using TodoList_Project.Core.DAL.Repositories;
using TodoList_Project.Core.MVVM;
using TodoList_Project.Core.Utils.Messages;
using TodoList_Project.Features.Categories.Services;
using TodoList_Project.Features.Categories.Views;
using TodoList_Project.Features.CharacterDialogs;
using TodoList_Project.Features.Tasks.Services;
using TodoList_Project.Features.Tasks.Views;
using Application = System.Windows.Application;

namespace TodoList_Project.Features.Main
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskService _taskService;
        private readonly ITaskFilterService _taskFilterService;
        private readonly ITaskStatisticsService _taskStatisticsService;
        private readonly ICategoryService _categoryService;
        private readonly IMessenger _messenger;

        [ObservableProperty]
        private ObservableObject currentChildView;

        [ObservableProperty]
        private string caption;

        [ObservableProperty]
        private IconChar icon;

        public MainViewModel(
            ITaskRepository taskRepository,
            ITaskService taskService,
            ITaskStatisticsService taskStatisticsService,
            ITaskFilterService taskFilterService,
            ICategoryService categoryService, IMessenger  messenger)
        {
            _taskRepository = taskRepository;
            _taskService = taskService;
            _taskStatisticsService = taskStatisticsService;
            _taskFilterService = taskFilterService;
            _categoryService = categoryService;
            ShowHomeView();
            CurrentChildView = new StartViewModel(_taskService,_taskStatisticsService, _taskFilterService);
           _messenger = messenger;
        }

        [RelayCommand]
        private void ShowHomeView()
        {
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        [RelayCommand]
        private void ShowCategoryManagementView()
        {
            CurrentChildView = new CategoryManagementViewModel(_categoryService);
            Caption = "CategoryManagementModel";
            Icon = IconChar.Icons;
        }

        [RelayCommand]
        private void ShowStartView()
        {
            CurrentChildView = new StartViewModel(_taskService,_taskStatisticsService,_taskFilterService);
            Caption = "Dashboard";
            Icon = IconChar.Calendar;
        }

        [RelayCommand]
        private void ShowTaskReportView()
        {
            CurrentChildView = new TaskReportViewModel(_taskStatisticsService);
            Caption = "Report";
            Icon = IconChar.PieChart;
        }

        [RelayCommand]
        private void ShowListTaskView()
        {
            CurrentChildView = new ListTaskViewModel(_taskService, _taskFilterService, _taskStatisticsService);
            Caption = "List Task";
            Icon = IconChar.ChartBar;
        }
        private void RegisterMessages()
        {
            _messenger.Register<ShowCharacterMessage>(this, (r, m) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var window = new CharacterDialogWindow
                    {
                        DataContext = new CharacterDialogViewModel(m.Value),
                        Owner = Application.Current.MainWindow
                    };

                    PositionWindow(window);
                    window.Show();

                    StartAutoCloseTimer(window);
                });
            });
        }

        private void PositionWindow(Window window)
        {
            var mainWindow = Application.Current.MainWindow;
            var mainWindowLocation = mainWindow.PointToScreen(new Point(0, 0));

            window.Left = mainWindowLocation.X + mainWindow.Width - window.Width - 20;
            window.Top = mainWindowLocation.Y + mainWindow.Height - window.Height - 20;
        }

        private void StartAutoCloseTimer(Window window)
        {
            Task.Delay(3000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(window.Close);
            });
        }
    }
}
