using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Input;
using TodoList_Project.Features.Tasks.Services;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Features.Tasks.Views
{
    public partial class TaskReportViewModel : ObservableObject
    {
        private readonly ITaskStatisticsService _statisticsService;

        [ObservableProperty]
        private SeriesCollection _seriesCollection;

        public TaskReportViewModel(ITaskStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
            SeriesCollection = new SeriesCollection();
        }

        public async Task LoadDataAsync(CancellationToken cancellationToken = default)
        {
            var distribution = await _statisticsService.GetTaskStatusDistributionAsync(cancellationToken);

            var collection = new SeriesCollection();

            foreach (var kvp in distribution)
            {
                collection.Add(new PieSeries
                {
                    Title = kvp.Key.ToString(),
                    Values = new ChartValues<int> { kvp.Value },
                    DataLabels = true
                });
            }

            SeriesCollection = collection;
        }
    }
}
