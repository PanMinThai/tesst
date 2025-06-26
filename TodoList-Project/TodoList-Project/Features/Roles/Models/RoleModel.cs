using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Roles.Models
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
