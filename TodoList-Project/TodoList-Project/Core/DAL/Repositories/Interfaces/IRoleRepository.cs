using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<RoleEntity, Guid>
    {
        Task<RoleEntity?> GetByNameAsync(string roleName);
        Task<IEnumerable<PermissionEntity>> GetPermissionsInRoleAsync(Guid roleId);
        Task<bool> AddPermissionToRoleAsync(Guid roleId, Guid permissionId);
        Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
    }
}
