using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Tasks.Models;

namespace TodoList_Project.Features.Tasks.Validation
{
    public class TaskValidator
    {
        public Dictionary<string, List<string>> Validate(TaskModel task)
        {
            var errors = new Dictionary<string, List<string>>();

            // Validate Title
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                AddError(errors, nameof(task.Title), "Title is required.");
            }
            else if (task.Title.Length > 100)
            {
                AddError(errors, nameof(task.Title), "Title must be less than 100 characters.");
            }

            // Validate DueDate
            if (task.DueDate < DateTime.Today)
            {
                AddError(errors, nameof(task.DueDate), "Due date must be in the future.");
            }

            return errors;
        }

        private void AddError(Dictionary<string, List<string>> errors, string propertyName, string message)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            errors[propertyName].Add(message);
        }
    }
}
