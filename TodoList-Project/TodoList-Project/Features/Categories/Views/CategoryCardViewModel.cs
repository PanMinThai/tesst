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
        private string _iconName = "Home";

        [ObservableProperty]
        private int _totalTasks = 12;

        [ObservableProperty]
        private int _completedTasks = 8;

        [ObservableProperty]
        private double _progressPercentage;
  
        [ObservableProperty]
        private SolidColorBrush _categoryBrush;

        public SolidColorBrush CardBackgroundBrush => GenerateLightBrush(0.85);  // Màu nền nhạt
        public SolidColorBrush CardBorderBrush => GenerateLightBrush(0.7);     // Màu viền
        public SolidColorBrush IconBackgroundBrush => GenerateLightBrush(0.95); // Màu nền icon
        public SolidColorBrush CategoryTextBrush => GenerateDarkBrush(0.2);           // Màu chữ
        public SolidColorBrush CategoryLightTextBrush => GenerateLightBrush(0.6); // Màu chữ nhạt
        public SolidColorBrush ProgressBackgroundBrush => GenerateLightBrush(0.45); // Màu nền progress

        public CategoryCardViewModel()
        {
            CalculateProgress();
        }

        partial void OnCategoryBrushChanged(SolidColorBrush value)
        {
            OnPropertyChanged(nameof(CardBackgroundBrush));
            OnPropertyChanged(nameof(CardBorderBrush));
            OnPropertyChanged(nameof(IconBackgroundBrush));
            OnPropertyChanged(nameof(CategoryTextBrush));
            OnPropertyChanged(nameof(CategoryLightTextBrush));
            OnPropertyChanged(nameof(ProgressBackgroundBrush));
        }

        partial void OnCompletedTasksChanged(int value) => CalculateProgress();
        partial void OnTotalTasksChanged(int value) => CalculateProgress();

        private void CalculateProgress()
        {
            ProgressPercentage = TotalTasks > 0 ? (CompletedTasks * 100.0 / TotalTasks) : 0;
        }

        private SolidColorBrush GenerateLightBrush(double lightnessFactor)
        {
            if (CategoryBrush == null) return new SolidColorBrush(Colors.White);

            Color baseColor = CategoryBrush.Color;
            var hsl = RgbToHsl(baseColor);
            hsl.L = Math.Min(0.95, hsl.L + (1 - hsl.L) * lightnessFactor); 
            return new SolidColorBrush(HslToRgb(hsl));
        }

        private SolidColorBrush GenerateDarkBrush(double darknessFactor)
        {
            if (CategoryBrush == null) return new SolidColorBrush(Colors.Black);

            Color baseColor = CategoryBrush.Color;
            var hsl = RgbToHsl(baseColor);
            hsl.L = hsl.L * (1 - darknessFactor);
            hsl.S = Math.Min(1, hsl.S * 1.2); 
            return new SolidColorBrush(HslToRgb(hsl));
        }
        private (double H, double S, double L) RgbToHsl(Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double h, s, l = (max + min) / 2;

            if (max == min)
            {
                h = s = 0; // achromatic
            }
            else
            {
                double d = max - min;
                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);

                if (max == r)
                    h = (g - b) / d + (g < b ? 6 : 0);
                else if (max == g)
                    h = (b - r) / d + 2;
                else
                    h = (r - g) / d + 4;

                h /= 6;
            }

            return (h, s, l);
        }

        private Color HslToRgb((double H, double S, double L) hsl)
        {
            double r, g, b;

            if (hsl.S == 0)
            {
                r = g = b = hsl.L; // achromatic
            }
            else
            {
                double q = hsl.L < 0.5 ? hsl.L * (1 + hsl.S) : hsl.L + hsl.S - hsl.L * hsl.S;
                double p = 2 * hsl.L - q;

                r = HueToRgb(p, q, hsl.H + 1.0 / 3);
                g = HueToRgb(p, q, hsl.H);
                b = HueToRgb(p, q, hsl.H - 1.0 / 3);
            }

            return Color.FromRgb(
                (byte)(r * 255),
                (byte)(g * 255),
                (byte)(b * 255));
        }

        private double HueToRgb(double p, double q, double t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;

            if (t < 1.0 / 6) return p + (q - p) * 6 * t;
            if (t < 1.0 / 2) return q;
            if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;

            return p;
        }
    }
}
