using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;

namespace TodoList_PhanMinhThai.Data
{
    // Data/ApplicationDbContext.cs
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<TaskCategoryEntity> TaskCategories { get; set; }

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
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TodoList;Trusted_Connection=True;TrustServerCertificate=True;"
);
        }
    }
}
