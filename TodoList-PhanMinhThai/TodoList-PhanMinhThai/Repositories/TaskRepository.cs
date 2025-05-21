using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_PhanMinhThai.Data.Entities;

namespace TodoList_PhanMinhThai.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task AddTaskAsync(Task task)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTaskAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync()
        {
            var tasks = new List<Task>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Tasks", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tasks.Add(new TaskEntity
                        {
                            Id = (int)reader["TaskId"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            DueDate = (DateTime)reader["DueDate"],
                            Status = reader["Status"].ToString(),
                            Priority = reader["Priority"].ToString(),
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]
                        });
                    }
                }
            }

            return tasks;
        }

        public Task<Task> GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task MarkTaskAsCompleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(Task task)
        {
            throw new NotImplementedException();
        }
    }

}
