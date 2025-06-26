using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Core.DAL.Repositories.Interfaces;

namespace TodoList_Project.Core.DAL.Repositories.Auth
{
    public class RoleRepository : GenericRepository<RoleEntity, Guid>, IRoleRepository
    {
        public RoleRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory) { }

        public async Task<RoleEntity?> GetByNameAsync(string roleName)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task<IEnumerable<PermissionEntity>> GetPermissionsInRoleAsync(Guid roleId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<bool> AddPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            if (await context.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId))
                return false;

            context.RolePermissions.Add(new RolePermissionEntity
            {
                RoleId = roleId,
                PermissionId = permissionId
            });

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var rolePermission = await context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null)
                return false;

            context.RolePermissions.Remove(rolePermission);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
