using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Roles.Models
{
    public class UserPermissionsModel
    {
        public Guid UserId { get; set; }
        public List<string> Permissions { get; set; }
    }
}
