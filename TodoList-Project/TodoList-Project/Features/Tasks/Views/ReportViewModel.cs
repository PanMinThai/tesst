using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TodoList_Project.Features.Tasks.Services;

namespace TodoList_Project.Features.Tasks.Views
{
    public partial class ReportViewModel : ObservableObject
    {
        //private readonly ITaskStatisticsService _taskStatisticsService;

        //// Properties for binding
        //[ObservableProperty]
        //private SeriesCollection _pieChartSeries;

        //[ObservableProperty]
        //private SeriesCollection _barChartSeries;

        //[ObservableProperty]
        //private SeriesCollection _lineChartSeries;

        //[ObservableProperty]
        //private string[] _priorityLabels;

        //[ObservableProperty]
        //private string[] _monthLabels;

        //[ObservableProperty]
        //private string[] _labels;

        //public ReportViewModel(ITaskStatisticsService taskStatisticsService)
        //{
        //    _taskStatisticsService = taskStatisticsService;
        //    InitializeData();
        //}

        //private void InitializeData()
        //{
        //    LoadPieChartData();
        //    LoadBarChartData();
        //    LoadLineChartData();
        //}

        //private void LoadPieChartData()
        //{
        //    var statistics = _taskStatisticsService.GetTaskStatistics();

        //    PieChartSeries = new SeriesCollection
        //{
        //    new PieSeries
        //    {
        //        Title = "In Progress",
        //        Values = new ChartValues<double> { statistics.InProgressCount },
        //        DataLabels = true,
        //        LabelPoint = point => $"{point.Y}",
        //        Fill = new SolidColorBrush(Color.FromRgb(0x4E, 0xCD, 0xC4)),
        //        Stroke = Brushes.White,
        //        StrokeThickness = 2
        //    },
        //    new PieSeries
        //    {
        //        Title = "Completed",
        //        Values = new ChartValues<double> { statistics.CompletedCount },
        //        DataLabels = true,
        //        LabelPoint = point => $"{point.Y}",
        //        Fill = new SolidColorBrush(Color.FromRgb(0x9B, 0x59, 0xB6)),
        //        Stroke = Brushes.White,
        //        StrokeThickness = 2
        //    },
        //    new PieSeries
        //    {
        //        Title = "Cancelled",
        //        Values = new ChartValues<double> { statistics.CancelledCount },
        //        DataLabels = true,
        //        LabelPoint = point => $"{point.Y}",
        //        Fill = new SolidColorBrush(Color.FromRgb(0xE7, 0x4C, 0x3C)),
        //        Stroke = Brushes.White,
        //        StrokeThickness = 2
        //    }
        //};
        //}

        //private void LoadBarChartData()
        //{
        //    var priorityStats = _taskStatisticsService.GetTaskPriorityStatistics();

        //    PriorityLabels = priorityStats.Select(p => p.Priority.ToString()).ToArray();

        //    BarChartSeries = new SeriesCollection
        //{
        //    new ColumnSeries
        //    {
        //        Title = "In Progress",
        //        Values = new ChartValues<double>(priorityStats.Select(p => (double)p.InProgressCount)),
        //        Fill = new SolidColorBrush(Color.FromRgb(0x4E, 0xCD, 0xC4)),
        //        DataLabels = true,
        //        LabelPoint = point => $"{point.Y}"
        //    },
        //    new ColumnSeries
        //    {
        //        Title = "Completed",
        //        Values = new ChartValues<double>(priorityStats.Select(p => (double)p.CompletedCount)),
        //        Fill = new SolidColorBrush(Color.FromRgb(0x9B, 0x59, 0xB6)),
        //        DataLabels = true,
        //        LabelPoint = point => $"{point.Y}"
        //    },
        //    new ColumnSeries
        //    {
        //        Title = "Cancelled",
        //        Values = new ChartValues<double>(priorityStats.Select(p => (double)p.CancelledCount)),
        //        Fill = new SolidColorBrush(Color.FromRgb(0xE7, 0x4C, 0x3C)),
        //        DataLabels = true,
        //        LabelPoint = point => $"{point.Y}"
        //    }
        //};
        //}

        //private void LoadLineChartData()
        //{
        //    var trendData = _taskStatisticsService.GetTaskTrendData();

        //    MonthLabels = trendData.Select(t => t.Month).ToArray();

        //    LineChartSeries = new SeriesCollection
        //{
        //    new LineSeries
        //    {
        //        Title = "In Progress",
        //        Values = new ChartValues<double>(trendData.Select(t => (double)t.InProgressCount)),
        //        Stroke = new SolidColorBrush(Color.FromRgb(0x4E, 0xCD, 0xC4)),
        //        Fill = Brushes.Transparent,
        //        PointGeometry = DefaultGeometries.Circle,
        //        PointGeometrySize = 10,
        //        PointForeground = new SolidColorBrush(Color.FromRgb(0x4E, 0xCD, 0xC4)),
        //        LineSmoothness = 0.2
        //    },
        //    new LineSeries
        //    {
        //        Title = "Completed",
        //        Values = new ChartValues<double>(trendData.Select(t => (double)t.CompletedCount)),
        //        Stroke = new SolidColorBrush(Color.FromRgb(0x9B, 0x59, 0xB6)),
        //        Fill = Brushes.Transparent,
        //        PointGeometry = DefaultGeometries.Circle,
        //        PointGeometrySize = 10,
        //        PointForeground = new SolidColorBrush(Color.FromRgb(0x9B, 0x59, 0xB6)),
        //        LineSmoothness = 0.2
        //    },
        //    new LineSeries
        //    {
        //        Title = "Cancelled",
        //        Values = new ChartValues<double>(trendData.Select(t => (double)t.CancelledCount)),
        //        Stroke = new SolidColorBrush(Color.FromRgb(0xE7, 0x4C, 0x3C)),
        //        Fill = Brushes.Transparent,
        //        PointGeometry = DefaultGeometries.Circle,
        //        PointGeometrySize = 10,
        //        PointForeground = new SolidColorBrush(Color.FromRgb(0xE7, 0x4C, 0x3C)),
        //        LineSmoothness = 0.2
        //    }
        //};
        //}

        //public void RefreshData()
        //{
        //    InitializeData();
        //}
    }
}