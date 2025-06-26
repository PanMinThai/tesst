using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Entities.Base
{
    public abstract class BaseEntity<T> : IEntity<T>
    {
        [Key]
        public T Id { get; set; }   
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
