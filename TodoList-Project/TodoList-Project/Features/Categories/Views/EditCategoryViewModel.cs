using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TodoList_Project.Features.Categories.Models;
using TodoList_Project.Features.Categories.Services;

namespace TodoList_Project.Features.Categories.Views
{
    public partial class EditCategoryViewModel : ObservableObject
    {
        private readonly CategoryCardViewModel _originalCard;
        private readonly Action _closeAction; 
        private readonly ICategoryService _categoryService;

        [ObservableProperty]
        private CategoryCardViewModel _categoryCardViewModel;

        [ObservableProperty]
        private string _customColorHex;

        public IRelayCommand<string> SelectColorCommand { get; }
        public IRelayCommand ApplyCustomColorCommand { get; }
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand CancelCommand { get; }
        public IRelayCommand CloseCommand { get; }

        public EditCategoryViewModel(CategoryCardViewModel cardViewModel, Action closeAction, ICategoryService categoryService)
        {
            _originalCard = cardViewModel;
            _closeAction = closeAction;
            _categoryService = categoryService;

            CategoryCardViewModel = new CategoryCardViewModel
            {
                Id = cardViewModel.Id,
                CategoryName = cardViewModel.CategoryName,
                IconName = cardViewModel.IconName,
                TotalTasks = cardViewModel.TotalTasks,
                CompletedTasks = cardViewModel.CompletedTasks,
                CategoryBrush = new SolidColorBrush(cardViewModel.CategoryBrush.Color),
                EditCommand = cardViewModel.EditCommand
            };

            CustomColorHex = CategoryCardViewModel.CategoryBrush.Color.ToString();

            SelectColorCommand = new RelayCommand<string>(OnSelectColor);
            ApplyCustomColorCommand = new RelayCommand(OnApplyCustomColor);
            SaveCommand = new AsyncRelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);
            CloseCommand = new RelayCommand(OnClose);
        }

        private void OnSelectColor(string colorName)
        {
            var color = colorName switch
            {
                "Blue" => Color.FromRgb(37, 99, 235),
                "Purple" => Color.FromRgb(147, 51, 234),
                "Pink" => Color.FromRgb(236, 72, 153),
                "Red" => Color.FromRgb(239, 68, 68),
                "Orange" => Color.FromRgb(249, 115, 22),
                "Yellow" => Color.FromRgb(234, 179, 8),
                "Green" => Color.FromRgb(34, 197, 94),
                "Teal" => Color.FromRgb(20, 184, 166),
                "Cyan" => Color.FromRgb(6, 182, 212),
                "Indigo" => Color.FromRgb(99, 102, 241),
                "Gray" => Color.FromRgb(107, 114, 128),
                "DarkGray" => Color.FromRgb(75, 85, 99),
                _ => Colors.Gray
            };

            // Cập nhật màu cho bản sao chỉnh sửa
            CategoryCardViewModel.CategoryBrush = new SolidColorBrush(color);
            CustomColorHex = color.ToString();
        }

        private void OnApplyCustomColor()
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(CustomColorHex);
                CategoryCardViewModel.CategoryBrush = new SolidColorBrush(color);
            }
            catch
            {
                CustomColorHex = CategoryCardViewModel.CategoryBrush.Color.ToString();
            }
        }

        private async Task OnSave() 
        {
            try
            {
                // Tạo DTO để cập nhật
                var updateDto = new CategoryDto
                {
                    Id = CategoryCardViewModel.Id,
                    Name = CategoryCardViewModel.CategoryName,
                    Icon = CategoryCardViewModel.IconName,
                    Color = CategoryCardViewModel.CategoryBrush.Color.ToString()
                };

                // Gọi service để cập nhật
                await _categoryService.UpdateCategoryAsync(updateDto);

                // Cập nhật UI
                _originalCard.CategoryName = CategoryCardViewModel.CategoryName;
                _originalCard.IconName = CategoryCardViewModel.IconName;
                _originalCard.CategoryBrush = new SolidColorBrush(CategoryCardViewModel.CategoryBrush.Color);

                _closeAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void OnCancel()
        {
            _closeAction?.Invoke();
        }

        private void OnClose()
        {
            _closeAction?.Invoke();
        }
    }
}
