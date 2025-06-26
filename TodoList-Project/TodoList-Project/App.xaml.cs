using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Repositories.Auth;
using TodoList_Project.Core.DAL.Repositories.General;
using TodoList_Project.Core.DAL.Repositories.Interfaces;
using TodoList_Project.Core.Utils.Mapper;
using TodoList_Project.Features.Auth.Services;
using TodoList_Project.Features.Auth.Services.Interfaces;
using TodoList_Project.Features.Auth.Views;
using TodoList_Project.Features.Categories.Services;
using TodoList_Project.Features.Categories.Views;
using TodoList_Project.Features.CharacterDialogs;
using TodoList_Project.Features.Main;
using TodoList_Project.Features.Main.Services;
using TodoList_Project.Features.Tasks.Services;
using TodoList_Project.Features.Tasks.Views;
using Application = System.Windows.Application;

namespace TodoList_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        public App()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            //    private readonly IConfiguration _configuration;

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog();
            });
            services.AddDbContextFactory<ApplicationDbContext>(options =>
        options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")),
        ServiceLifetime.Transient);
            // Repositories
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IMessageTemplateRepository, MessageTemplateRepository>();
            services.AddTransient<ICharacterIconRepository, CharacterIconRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            //Services
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<ITaskFilterService, TaskFilterService>();
            services.AddTransient<ITaskStatisticsService, TaskStatisticsService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IPasswordService, PasswordService>();
            //Mapper
            services.AddAutoMapper(typeof(TaskMappingProfile));

            // ViewModels
            
            
            services.AddTransient<MainViewModel>();
            services.AddTransient<StartViewModel>();
            services.AddTransient<TaskItemViewModel>();
            services.AddTransient<UndoableTaskItemViewModel>();
            services.AddTransient<ListTaskViewModel>();
            services.AddTransient<TaskReportViewModel>();
            services.AddTransient<CategoryCardViewModel>();
            services.AddTransient<EditCategoryViewModel>();
            services.AddTransient<CategoryManagementViewModel>();
            services.AddTransient<AuthViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            //services.AddTransient<TaskViewModel>();
            // Views
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainView>();
            services.AddSingleton<AuthView>();
            services.AddTransient<CharacterDialogWindow>();
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            //var homeWindow = _serviceProvider.GetRequiredService<MainView>();
            //homeWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            //homeWindow.Show();

            base.OnStartup(e);
            var startWindow = _serviceProvider.GetRequiredService<AuthView>();
            startWindow.DataContext = _serviceProvider.GetRequiredService<AuthViewModel>();
            startWindow.Show();
        }
    }

}
