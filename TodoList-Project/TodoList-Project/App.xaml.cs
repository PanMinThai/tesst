using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Repositories;
using TodoList_Project.Core.Utils.Mapper;
using TodoList_Project.Features.Main;
using TodoList_Project.Features.Tasks.Services;
using Application = System.Windows.Application;

namespace TodoList_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Server=.\\SQLEXPRESS;Database=TodoList;Trusted_Connection=True;TrustServerCertificate=True;"));

            // Repositories
            services.AddScoped<ITaskRepository, TaskRepository>();

            //Services
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskFilterService, TaskFilterService>();
            services.AddScoped<ITaskStatisticsService, TaskStatisticsService>();
            //Mapper
            services.AddAutoMapper(typeof(TaskMappingProfile));

            // ViewModels
            
            services.AddTransient<MainViewModel>();
            //services.AddTransient<StartViewModel>();
            //services.AddTransient<TaskItemViewModel>();
            //services.AddTransient<TaskItemViewModel>();
            //services.AddTransient<ListTaskViewModel>();
            //services.AddTransient<TaskViewModel>();
            // Views
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var homeWindow = _serviceProvider.GetRequiredService<MainView>();
            homeWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            homeWindow.Show();
        }
    }

}
