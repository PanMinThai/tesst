using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public interface IPermissionRepository : IGenericRepository<PermissionEntity, Guid>
    {
        Task<PermissionEntity?> GetByNameAsync(string permissionName);
        Task<IEnumerable<RoleEntity>> GetRolesWithPermissionAsync(Guid permissionId);
        Task<bool> PermissionExistsInRole(Guid permissionId, Guid roleId);
    }
}
