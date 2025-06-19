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
    public class MessageTemplateEntity : BaseEntity<int>
    {
        [Required]
        public Tone Tone { get; set; } 

        [Required]
        [StringLength(300)]
        public string Content { get; set; }
    }

}
