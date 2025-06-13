using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TodoList_Project.Features.Categories.Views
{
    public partial class CategoryCardViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _categoryName = "Work";

        [ObservableProperty]
        private string _iconText = ""; // MDL2 icon code for briefcase

        [ObservableProperty]
        private int _totalTasks = 12;

        [ObservableProperty]
        private int _completedTasks = 8;

        [ObservableProperty]
        private double _progressPercentage;

        [ObservableProperty]
        private SolidColorBrush _categoryBrush = new SolidColorBrush(Color.FromRgb(59, 130, 246));

        [ObservableProperty]
        private SolidColorBrush _cardBackgroundBrush = new SolidColorBrush(Color.FromRgb(239, 246, 255));

        [ObservableProperty]
        private SolidColorBrush _cardBorderBrush = new SolidColorBrush(Color.FromRgb(219, 234, 254));

        [ObservableProperty]
        private SolidColorBrush _iconBackgroundBrush = new SolidColorBrush(Color.FromRgb(239, 246, 255));

        [ObservableProperty]
        private SolidColorBrush _categoryTextBrush = new SolidColorBrush(Color.FromRgb(59, 130, 246));

        [ObservableProperty]
        private SolidColorBrush _categoryLightTextBrush = new SolidColorBrush(Color.FromRgb(100, 116, 139));

        [ObservableProperty]
        private SolidColorBrush _progressBackgroundBrush = new SolidColorBrush(Color.FromRgb(226, 232, 240));

        public CategoryCardViewModel()
        {
            CalculateProgress();
        }

        partial void OnCompletedTasksChanged(int value)
        {
            CalculateProgress();
        }

        partial void OnTotalTasksChanged(int value)
        {
            CalculateProgress();
        }

        private void CalculateProgress()
        {
            ProgressPercentage = TotalTasks > 0 ? (CompletedTasks * 100.0 / TotalTasks) : 0;
        }
    }
}
