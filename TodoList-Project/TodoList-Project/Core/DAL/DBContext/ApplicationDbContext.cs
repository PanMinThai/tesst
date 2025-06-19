using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL;

namespace TodoList_Project.Core.DAL.DBContext
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<TaskCategoryEntity> TaskCategories { get; set; }

        public DbSet<MessageTemplateEntity> MessageTemplates { get; set; }
        public DbSet<CharacterIconEntity> CharacterIcons { get; set; }
        public DbSet<UserFeedbackEntity> UserFeedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TaskCategory as junction table with composite primary key
            modelBuilder.Entity<TaskCategoryEntity>()
                .HasKey(tc => new { tc.TaskId, tc.CategoryId });

            modelBuilder.Entity<TaskCategoryEntity>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.TaskCategories)
                .HasForeignKey(tc => tc.TaskId);

            modelBuilder.Entity<TaskCategoryEntity>()
                .HasOne(tc => tc.Category)
                .WithMany(c => c.TaskCategories)
                .HasForeignKey(tc => tc.CategoryId);

            // Configure enum conversions
            modelBuilder.Entity<TaskEntity>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<TaskEntity>()
                .Property(t => t.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<MessageTemplateEntity>()
                .Property(m => m.Tone)
                .HasConversion<string>();

            modelBuilder.Entity<CharacterIconEntity>()
                .Property(i => i.Tone)
                .HasConversion<string>();

            modelBuilder.Entity<UserFeedbackEntity>()
                .Property(f => f.Tone)
                .HasConversion<string>();
        }
    }
}