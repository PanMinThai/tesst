using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using TodoList_PhanMinhThai.Data;
using TodoList_PhanMinhThai.Repositories;
using TodoList_PhanMinhThai.ViewModels;
using TodoList_PhanMinhThai.Views;
using Application = System.Windows.Application;

namespace TodoList_PhanMinhThai
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
            services.AddScoped<ITaskRepository, TaskRepository>();

            // ViewModels
            services.AddTransient<TaskViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<StartViewModel>();
            services.AddTransient<TaskItemViewModel>();
            services.AddTransient<TaskItemViewModel>();

            // Views
            services.AddSingleton<MainWindow>();
            services.AddSingleton<HomeView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            //mainWindow.DataContext = _serviceProvider.GetRequiredService<TaskViewModel>();
            //mainWindow.Show();
            var homeWindow = _serviceProvider.GetRequiredService<HomeView>();
            homeWindow.DataContext = _serviceProvider.GetRequiredService<HomeViewModel>();
            homeWindow.Show();
        }
    }

}