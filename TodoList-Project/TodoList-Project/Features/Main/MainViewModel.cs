using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TodoList_Project.Core.DAL.Repositories;
using TodoList_Project.Core.MVVM;
using TodoList_Project.Features.Tasks.Services;
using TodoList_Project.Features.Tasks.Views;

namespace TodoList_Project.Features.Main
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskService _taskService;
        private readonly ITaskFilterService _taskFilterService;
        private readonly ITaskStatisticsService _taskStatisticsService;

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
            ITaskFilterService taskFilterService)
        {
            _taskRepository = taskRepository;
            _taskService = taskService;
            _taskStatisticsService = taskStatisticsService;
            _taskFilterService = taskFilterService;

            ShowHomeView();
            CurrentChildView = new StartViewModel(_taskService);
        }

        [RelayCommand]
        private void ShowHomeView()
        {
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        [RelayCommand]
        private void ShowAddNewTaskView()
        {
            //CurrentChildView = new AddNewTaskViewModel();
            Caption = "AddTaskModel";
            Icon = IconChar.UserGroup;
        }

        [RelayCommand]
        private void ShowStartView()
        {
            CurrentChildView = new StartViewModel(_taskService);
            Caption = "Dashboard";
            Icon = IconChar.Calendar;
        }

        [RelayCommand]
        private void ShowTaskManagementView()
        {
            //CurrentChildView = new TaskViewModel(_taskService);
            Caption = "Dashboard";
            Icon = IconChar.ListCheck;
        }

        [RelayCommand]
        private void ShowListTaskView()
        {
            CurrentChildView = new ListTaskViewModel(_taskService, _taskFilterService, _taskStatisticsService);
            Caption = "List Task";
            Icon = IconChar.ChartBar;
        }
    }
}
