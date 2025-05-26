using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data;
using TodoList_PhanMinhThai.Data.Entities;
using TodoList_PhanMinhThai.Dtos;
using TodoList_PhanMinhThai.Models;
using TaskStatus = TodoList_PhanMinhThai.Data.Entities.TaskStatus;

namespace TodoList_PhanMinhThai.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public int GetInProgressCount()
        {
            return _context.Tasks.Count(t => t.Status == TaskStatus.InProgress);
        }

        // 2. Hàm đếm task đã hoàn thành (Completed)
        public int GetCompletedCount()
        {
            return _context.Tasks.Count(t => t.Status == TaskStatus.Completed);
        }

        // 3. Hàm đếm task đã hủy (Cancelled)
        public int GetCancelledCount()
        {
            return _context.Tasks.Count(t => t.Status == TaskStatus.Cancelled);
        }
        public async Task AddTaskAsync(TaskModel task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            try
            {
                // Validate dữ liệu trước khi thêm
                if (string.IsNullOrWhiteSpace(task.Title))
                    throw new ArgumentException("Task title cannot be empty");

                var taskEntity = new TaskEntity
                {
                    Title = task.Title.Trim(),
                    DueDate = task.DueDate,
                    Status = task.Status,
                    Priority = task.Priority,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _context.Tasks.AddAsync(taskEntity);
                await _context.SaveChangesAsync();

                // Cập nhật ID trả về
                task.Id = taskEntity.Id;
                task.CreatedAt = taskEntity.CreatedAt;
                task.UpdatedAt = taskEntity.UpdatedAt;
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Database error while adding task", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to add task", ex);
            }
        }

        public async Task DeleteTaskAsync(int id)
        {
            try
            {
                // Tìm task cần xóa
                var taskEntity = await _context.Tasks.FindAsync(id);

                if (taskEntity == null)
                {
                    throw new KeyNotFoundException($"Task with ID {id} not found");
                }

                // Xóa task
                _context.Tasks.Remove(taskEntity);

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Database error while deleting task", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to delete task", ex);
            }
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Select(t => new TaskModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    Priority = t.Priority,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();
        }

        public Task<Task> GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTaskAsync(TaskModel task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            try
            {
                var existingTask = await _context.Tasks.FindAsync(task.Id);

                if (existingTask == null)
                    throw new KeyNotFoundException($"Task with ID {task.Id} not found");

                // Cập nhật thông tin
                existingTask.Title = task.Title?.Trim();
                existingTask.DueDate = task.DueDate;
                existingTask.Status = task.Status;
                existingTask.Priority = task.Priority;
                existingTask.UpdatedAt = DateTime.Now;

                _context.Tasks.Update(existingTask);
                await _context.SaveChangesAsync();

                // Cập nhật lại thông tin cho model nếu cần
                task.UpdatedAt = existingTask.UpdatedAt;
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Database error while updating task", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to update task", ex);
            }
        }

    }

}
