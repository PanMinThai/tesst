using Microsoft.EntityFrameworkCore;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Core.DAL.Repositories.Interfaces;

namespace TodoList_Project.Core.DAL.Repositories.Auth
{
    public class PermissionRepository : GenericRepository<PermissionEntity, Guid>, IPermissionRepository
    {
        public PermissionRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory) { }

        public async Task<PermissionEntity?> GetByNameAsync(string permissionName)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Permissions
                .FirstOrDefaultAsync(p => p.Name == permissionName);
        }

        public async Task<IEnumerable<RoleEntity>> GetRolesWithPermissionAsync(Guid permissionId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .Select(rp => rp.Role)
                .ToListAsync();
        }

        public async Task<bool> PermissionExistsInRole(Guid permissionId, Guid roleId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.RolePermissions
                .AnyAsync(rp => rp.PermissionId == permissionId && rp.RoleId == roleId);
        }
    }
}
