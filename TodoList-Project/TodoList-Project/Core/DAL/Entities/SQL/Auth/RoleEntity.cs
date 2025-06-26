using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.Base;

namespace TodoList_Project.Core.DAL.Entities.SQL.Auth
{
    public class RoleEntity : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
        public ICollection<RolePermissionEntity> RolePermissions { get; set; } = new List<RolePermissionEntity>();
    }
}
