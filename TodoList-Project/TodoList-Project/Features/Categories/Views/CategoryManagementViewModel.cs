using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Categories.Services;
using TodoList_Project.Features.Categories.Views;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Categories.Views
{
    public partial class CategoryManagementViewModel : ObservableObject
    {
        private string _selectedPeriod = "Today";
        private readonly ICategoryService _categoryService;
        private bool _isLoading;
        [ObservableProperty]
        private bool _isEditPopupVisible;
        [ObservableProperty]
        private EditCategoryViewModel? _editCategoryViewModel;
        private CategoryCardViewModel _selectedCategory;

        public CategoryManagementViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            SelectPeriodCommand = new RelayCommand<string>(OnPeriodSelected);
            LoadCategoriesCommand = new AsyncRelayCommand(LoadCategoriesAsync);
            EditCategoryCommand = new RelayCommand<CategoryCardViewModel>(OnEditCategory);

            _ = LoadCategoriesAsync();
        }

        #region Properties

        public ObservableCollection<CategoryCardViewModel> Categories { get; } = new ObservableCollection<CategoryCardViewModel>();

        public string SelectedPeriod
        {
            get => _selectedPeriod;
            set => SetProperty(ref _selectedPeriod, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }


        public CategoryCardViewModel SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
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
        public ICommand EditCategoryCommand { get; }


        #endregion

        #region Methods
        [RelayCommand]
        private void CloseEditPopup()
        {
            IsEditPopupVisible = false;
            EditCategoryViewModel = null;
        }
        private async void OnPeriodSelected(string period)
        {
            SelectedPeriod = period;
            await LoadCategoriesAsync();
        }

        private void OnEditCategory(CategoryCardViewModel? category)
        {
            if (category == null) return;

            SelectedCategory = category;
            EditCategoryViewModel = new EditCategoryViewModel(category, CloseEditPopup,_categoryService);
            IsEditPopupVisible = true;
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

                    var vm = new CategoryCardViewModel
                    {
                        Id = category.Id,
                        CategoryName = category.Name,
                        IconName = category.Icon,
                        TotalTasks = tasksInPeriod,
                        CompletedTasks = completedTasksInPeriod,
                        CategoryBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(category.Color)),
                        EditCommand = new RelayCommand<CategoryCardViewModel>(OnEditCategory)
                    };

                    Categories.Add(vm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading categories: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                UpdatePeriodFlags();
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