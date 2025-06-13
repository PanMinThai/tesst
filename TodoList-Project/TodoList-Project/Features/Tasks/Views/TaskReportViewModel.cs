using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Features.Tasks.Services;
using Application = System.Windows.Application;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Views
{
    public partial class TaskReportViewModel : ObservableObject
    {
        private readonly ITaskStatisticsService _statisticsService;

        private readonly Brush[] _brushes =
        {
            (Brush)Application.Current.FindResource("PieItem1Brush"),
            (Brush)Application.Current.FindResource("PieItem2Brush"),
            (Brush)Application.Current.FindResource("PieItem3Brush"),
        };
        private readonly Brush[] _barBrushes =
        {
            (Brush)Application.Current.FindResource("BarItem1Brush"), // In Progress
            (Brush)Application.Current.FindResource("BarItem2Brush"), // Completed
            (Brush)Application.Current.FindResource("BarItem3Brush")  // Cancelled
        };
        private DateTimePeriod _selectedPeriod = DateTimePeriod.LastWeek;
        public DateTimePeriod SelectedPeriod
        {
            get => _selectedPeriod;
            set => SetProperty(ref _selectedPeriod, value);
        }
        private DateTime? _customStartDate;
        public DateTime? CustomStartDate
        {
            get => _customStartDate;
            set => SetProperty(ref _customStartDate, value);
        }
        private DateTime? _customEndDate;
        public DateTime? CustomEndDate
        {
            get => _customEndDate;
            set => SetProperty(ref _customEndDate, value);
        }

        [ObservableProperty]
        private SeriesCollection _pieSeriesCollection;
        [ObservableProperty]
        private SeriesCollection _barSeriesCollection;
        [ObservableProperty]
        private string[] _barLabels = { "Low Priority", "Medium Priority", "High Priority" };
        [ObservableProperty]
        private SeriesCollection _lineSeriesCollection;
        [ObservableProperty]
        private string[] _lineLabels;

        public ICommand ChangePeriodCommand { get; }
        public ICommand LoadDataCommand { get; } 

        public TaskReportViewModel(ITaskStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
            PieSeriesCollection = new SeriesCollection();
            BarSeriesCollection = new SeriesCollection();
            LineSeriesCollection = new SeriesCollection();

            ChangePeriodCommand = new RelayCommand<DateTimePeriod>(async (period) =>
            {
                SelectedPeriod = period;
                await LoadDataAsync();
            });

            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
             await LoadPieChartDataAsync();
             await LoadBarChartDataAsync();
             await LoadLineChartDataAsync();
        }
        private async Task LoadPieChartDataAsync()
        {
            var borderBrush = (Brush)Application.Current.FindResource("panelColor");
            var distribution = await _statisticsService.GetTaskStatusDistributionAsync(SelectedPeriod, CustomStartDate, CustomEndDate);

            var collection = new SeriesCollection();
            int index = 0;

            foreach (var kvp in distribution)
            {
                var series = new PieSeries
                {
                    Title = kvp.Key.ToString(),
                    Values = new ChartValues<int> { kvp.Value },
                    DataLabels = true,
                    StrokeThickness = 5,
                    Stroke = borderBrush,
                    Fill = _brushes[index % _brushes.Length]
                };
                collection.Add(series);
                index++;
            }
            PieSeriesCollection = collection;
        }
        private async Task LoadBarChartDataAsync()
        {
            var borderBrush = (Brush)Application.Current.FindResource("panelColor");
            var priorityStatusData = await _statisticsService.GetTasksByPriorityAndStatusAsync(SelectedPeriod, CustomStartDate, CustomEndDate);
            var barCollection = new SeriesCollection();

            var statuses = new[] { TaskStatus.InProgress, TaskStatus.Completed, TaskStatus.Cancelled };
            for (int i = 0; i < statuses.Length; i++)
            {
                var values = new ChartValues<int>();
                foreach (var priority in Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>())
                {
                    values.Add(priorityStatusData.ContainsKey(priority) && priorityStatusData[priority].ContainsKey(statuses[i])
                        ? priorityStatusData[priority][statuses[i]]
                        : 0);
                }

                var series = new ColumnSeries
                {
                    Title = statuses[i].ToString(),
                    Values = values,
                    Fill = _barBrushes[i % _barBrushes.Length],
                    StrokeThickness = 1,
                    Stroke = borderBrush
                };
                barCollection.Add(series);
            }
            BarSeriesCollection = barCollection;
        }
        private async Task LoadLineChartDataAsync()
        {
            var borderBrush = (Brush)Application.Current.FindResource("panelColor");

            var taskCountByDate = await _statisticsService.GetTaskCountByDateAsync(SelectedPeriod, CustomStartDate, CustomEndDate);

            var lineCollection = new SeriesCollection();
            var values = new ChartValues<int>(taskCountByDate.Values);
            var labels = taskCountByDate.Keys.Select(d => d.ToString("dd/MM")).ToArray();

            // Tạo LinearGradientBrush cho Fill
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0), 
                EndPoint = new Point(1, 0),  
                Opacity = 0.5,               
                GradientStops = new GradientStopCollection
                {
                    new GradientStop((Color)ColorConverter.ConvertFromString("#F1587F"), 0),
                    new GradientStop((Color)ColorConverter.ConvertFromString("#6B53FF"), 1) 
                }
            };

            var series = new LineSeries
            {
                Title = "Total Tasks",
                Values = values,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8080")), 
                StrokeThickness = 1,
                Fill = gradientBrush,
                PointGeometry = null 
            };
            lineCollection.Add(series);

            LineSeriesCollection = lineCollection;
            LineLabels = labels;
        }
    }
}
