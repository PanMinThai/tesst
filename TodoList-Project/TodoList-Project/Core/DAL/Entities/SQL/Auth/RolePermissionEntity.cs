using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Entities.SQL.Auth
{
    public class RolePermissionEntity
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public RoleEntity Role { get; set; }
        public PermissionEntity Permission { get; set; }
    }
}
