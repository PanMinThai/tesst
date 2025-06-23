using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.Base;
using TodoList_Project.Core.DAL.Enums;

namespace TodoList_Project.Core.DAL.Entities.SQL
{
    public class UserFeedbackEntity : BaseEntity<int>
    {
        [Required]
        public string ActionType { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public Tone Tone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

}
