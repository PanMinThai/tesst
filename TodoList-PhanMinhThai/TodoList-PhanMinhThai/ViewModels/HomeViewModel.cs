using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList_PhanMinhThai.Repositories;
using TodoList_PhanMinhThai.Services;

namespace TodoList_PhanMinhThai.ViewModels
{
    class HomeViewModel :ViewModelBase
    {
        ////private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
        private ITaskRepository taskRepository;

        private ITaskService _taskService;
        private ITaskFilterService _taskFilterService;
        private ITaskStatisticsService _taskStatisticsService;
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
    
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowAddNewTaskViewCommand { get; }
        public ICommand ShowStartViewCommand { get; }
        public ICommand ShowTaskManagementViewCommand { get; }
        public ICommand ShowListTaskViewCommand { get; }
        public HomeViewModel(ITaskRepository taskRepository, ITaskService taskService, ITaskStatisticsService taskStatisticsService, ITaskFilterService taskFilterService)
        {
            this.taskRepository = taskRepository;
            _taskFilterService = taskFilterService;
            _taskService = taskService;
            _taskStatisticsService = taskStatisticsService;

            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowAddNewTaskViewCommand = new ViewModelCommand(ExecuteAddNewTaskViewCommand);
            ShowStartViewCommand = new ViewModelCommand(ExecuteShowStartViewCommand);
            ShowTaskManagementViewCommand = new ViewModelCommand(ExecuteTaskManagementViewCommand);
            ShowListTaskViewCommand = new ViewModelCommand(ExecuteListTaskViewCommand);

            ExecuteShowHomeViewCommand(null);
            CurrentChildView = new StartViewModel(_taskService);
        }
        private void ExecuteAddNewTaskViewCommand(object obj)
        {
            CurrentChildView = new AddNewTaskViewModel();
            Caption = "AddTaskModel";
            Icon = IconChar.UserGroup;
        }
        private void ExecuteShowHomeViewCommand(object obj)
        {
          //  CurrentChildView = new TaskViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;   
        }
        private void ExecuteShowStartViewCommand(object obj)
        {
            CurrentChildView = new StartViewModel(_taskService);
            Caption = "Dashboard";  
            Icon = IconChar.Calendar;
        }
        private void ExecuteTaskManagementViewCommand(object obj)
        {   
            CurrentChildView = new TaskViewModel(_taskService);
            Caption = "Dashboard";
            Icon = IconChar.ListCheck;
        }
        private void ExecuteListTaskViewCommand(object obj)
        {
            CurrentChildView = new ListTaskViewModel(_taskService,_taskFilterService,_taskStatisticsService);
            Caption = "List Task";
            Icon = IconChar.ChartBar;
        }
    }
}
