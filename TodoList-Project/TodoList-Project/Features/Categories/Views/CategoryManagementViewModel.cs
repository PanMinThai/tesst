using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Categories.Services;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Categories.Views
{
    public class CategoryManagementViewModel: ObservableObject
    {
        private string _selectedPeriod = "Today";
        private readonly ICategoryService _categoryService;
        private bool _isLoading;

        public CategoryManagementViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            SelectPeriodCommand = new RelayCommand<string>(OnPeriodSelected);
            LoadCategoriesCommand = new AsyncRelayCommand(LoadCategoriesAsync);

            // Load categories on initialization
            _ = LoadCategoriesAsync();
        }

        #region Properties

        public ObservableCollection<CategoryCardViewModel> Categories { get; set; } = new ObservableCollection<CategoryCardViewModel>();

        public string SelectedPeriod
        {
            get => _selectedPeriod;
            set
            {
                SetProperty(ref _selectedPeriod, value);
                UpdatePeriodFlags();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        // Period flags for button states
        public bool IsToday => SelectedPeriod == "Today";
        public bool IsYesterday => SelectedPeriod == "Yesterday";
        public bool IsThisWeek => SelectedPeriod == "ThisWeek";
        public bool IsLastWeek => SelectedPeriod == "LastWeek";
        public bool IsThisMonth => SelectedPeriod == "ThisMonth";
        public bool IsLastMonth => SelectedPeriod == "LastMonth";

        #endregion

        #region Commands

        public ICommand SelectPeriodCommand { get; }
        public ICommand LoadCategoriesCommand { get; }

        #endregion

        #region Methods

        private async void OnPeriodSelected(string period)
        {
            SelectedPeriod = period;
            await LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                IsLoading = true;

                var period = SelectedPeriod switch
                {
                    "Today" => DateTimePeriod.Today,
                    "Yesterday" => DateTimePeriod.Yesterday,
                    "ThisWeek" => DateTimePeriod.ThisWeek,
                    "LastWeek" => DateTimePeriod.LastWeek,
                    "ThisMonth" => DateTimePeriod.ThisMonth,
                    "LastMonth" => DateTimePeriod.LastMonth,
                    _ => DateTimePeriod.All
                };

                var categories = await _categoryService.GetAllCategoriesAsync(period);

                Categories.Clear();
                foreach (var category in categories)
                {
                    var tasksInPeriod = category.TaskCategories?.Count ?? 0;
                    var completedTasksInPeriod = category.TaskCategories?
                        .Count(tc => tc.Task?.Status == TaskStatus.Completed) ?? 0;

                    Categories.Add(new CategoryCardViewModel
                    {
                        CategoryName = category.Name,
                        IconName = category.Icon,
                        TotalTasks = tasksInPeriod,
                        CompletedTasks = completedTasksInPeriod,
                        CategoryBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(category.Color))
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading categories: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        private void UpdatePeriodFlags()
        {
            OnPropertyChanged(nameof(IsToday));
            OnPropertyChanged(nameof(IsYesterday));
            OnPropertyChanged(nameof(IsThisWeek));
            OnPropertyChanged(nameof(IsLastWeek));
            OnPropertyChanged(nameof(IsThisMonth));
            OnPropertyChanged(nameof(IsLastMonth));
        }

        #endregion
    }
}
