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
using TodoList_Project.Features.Categories.Services;
using TodoList_Project.Features.Categories.Views;
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
        private readonly ICategoryService _categoryService;

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
            ICategoryService categoryService)
        {
            _taskRepository = taskRepository;
            _taskService = taskService;
            _taskStatisticsService = taskStatisticsService;
            _taskFilterService = taskFilterService;
            _categoryService = categoryService;
            ShowHomeView();
            CurrentChildView = new StartViewModel(_taskService,_taskStatisticsService, _taskFilterService);
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
    }
}
