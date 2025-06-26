using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.Base;

namespace TodoList_Project.Core.DAL.Entities.SQL.Auth
{
    public class PermissionEntity : BaseEntity<Guid>
    {
        public string Name { get; set; } // "CreateTask", "DeleteUser"
        public string Description { get; set; }

        // Navigation properties
        public ICollection<RolePermissionEntity> RolePermissions { get; set; }
    }

}
