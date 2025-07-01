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
        Task<RoleEntity> GetByNameAsync(string name);
        Task<bool> RoleExistsAsync(string name);
        Task<IEnumerable<PermissionEntity>> GetPermissionsForRoleAsync(Guid roleId);
        Task<IEnumerable<PermissionEntity>> GetPermissionsForRolesAsync(IEnumerable<Guid> roleIds);
        Task<bool> AddPermissionToRoleAsync(Guid roleId, Guid permissionId);
        Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
        Task<bool> HasPermissionAsync(Guid roleId, string permissionName);
        Task<IEnumerable<UserEntity>> GetUsersInRoleAsync(Guid roleId);
        Task<bool> IsUserInRoleAsync(Guid userId, Guid roleId);
        Task<bool> AddUserToRoleAsync(Guid userId, Guid roleId);
        Task<bool> RemoveUserFromRoleAsync(Guid userId, Guid roleId);
    }
}
