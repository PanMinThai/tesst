using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Entities.Base
{
    public interface IEntity<T>
    {
        T Id { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
