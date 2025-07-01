using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.INI;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;

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
        public DbSet<UserFeedbackEntity> UserFeedbacks { get; set; } // Update later: add 1 property to connect with UserEntity :))
        // Authentication and Authorization entities
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }
        public DbSet<LoginHistoryEntity> LoginHistories { get; set; }
        public DbSet<PasswordResetTokenEntity> PasswordResetTokens { get; set; }
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

            // ---- User Configuration ----
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.DisplayName).HasMaxLength(100);
            });

            // ---- Role Configuration ----
            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            });

            // ---- User-Role (Many-to-Many) ----
            modelBuilder.Entity<UserRoleEntity>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---- Permission Configuration ----
            modelBuilder.Entity<PermissionEntity>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);
            });

            // ---- Role-Permission (Many-to-Many) ----
            modelBuilder.Entity<RolePermissionEntity>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---- PasswordResetToken Configuration ----
            modelBuilder.Entity<PasswordResetTokenEntity>(entity =>
            {
                entity.Property(t => t.Token).IsRequired().HasMaxLength(500);
                entity.HasIndex(t => t.Token).IsUnique();

                entity.HasOne(t => t.User)
                    .WithMany(u => u.PasswordResetTokens)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---- LoginHistory Configuration ----
            modelBuilder.Entity<LoginHistoryEntity>(entity =>
            {
                entity.HasOne(l => l.User)
                    .WithMany(u => u.LoginHistories)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---- Task Configuration ----
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.Property(t => t.Title).IsRequired().HasMaxLength(100);

                entity.HasOne(t => t.AssignedUser)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.AssignedUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---- Seed Data ----
            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            var adminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var userRoleId = Guid.Parse("00000000-0000-0000-0000-000000000002");

            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity { Id = adminRoleId, Name = "Admin" },
                new RoleEntity { Id = userRoleId, Name = "User" }
            );

            // Seed Permissions
            var manageTasksPermId = Guid.Parse("00000000-0000-0000-0000-000000000003");
            var manageUsersPermId = Guid.Parse("00000000-0000-0000-0000-000000000004");

            modelBuilder.Entity<PermissionEntity>().HasData(
                new PermissionEntity
                {
                    Id = manageTasksPermId,
                    Name = "ManageTasks",
                    Description = "Quản lý công việc"
                },
                new PermissionEntity
                {
                    Id = manageUsersPermId,
                    Name = "ManageUsers",
                    Description = "Quản lý người dùng"
                }
            );

            // Assign Permissions to Roles
            modelBuilder.Entity<RolePermissionEntity>().HasData(
                new RolePermissionEntity { RoleId = adminRoleId, PermissionId = manageTasksPermId },
                new RolePermissionEntity { RoleId = adminRoleId, PermissionId = manageUsersPermId },
                new RolePermissionEntity { RoleId = userRoleId, PermissionId = manageTasksPermId }
            );

            // Seed Admin User
            var adminUserId = Guid.Parse("00000000-0000-0000-0000-000000000005");
            var salt = "$2a$11$BrU23Rk9TXFDmSlzWYPVvO";
            var passwordHash = "$2a$11$BrU23Rk9TXFDmSlzWYPVvOBS.pH7LjAkvF.2UrF0kiErs84BFIw7a";
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = adminUserId,
                    Email = "admin@gmail.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    DisplayName = "Quản trị viên",
                    EmailConfirmed = true,
                    IsLocked = false,
                    FailedLoginAttempts = 0,
                    LastLogin = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                    UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                }
            );

            // Assign Admin Role to Admin User
            modelBuilder.Entity<UserRoleEntity>().HasData(
                new UserRoleEntity { UserId = adminUserId, RoleId = adminRoleId }
            );
        }
    }
}