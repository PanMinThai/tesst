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
            : base(contextFactory)
        {
        }

        public async Task<RoleEntity> GetByNameAsync(string name)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<RoleEntity>()
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<bool> RoleExistsAsync(string name)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<RoleEntity>()
                .AnyAsync(r => r.Name == name);
        }

        public async Task<IEnumerable<PermissionEntity>> GetPermissionsForRoleAsync(Guid roleId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<RolePermissionEntity>()
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<PermissionEntity>> GetPermissionsForRolesAsync(IEnumerable<Guid> roleIds)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<RolePermissionEntity>()
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> AddPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            // Check if the permission already exists for the role
            var exists = await context.Set<RolePermissionEntity>()
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (exists) return false;

            var rolePermission = new RolePermissionEntity
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            context.Set<RolePermissionEntity>().Add(rolePermission);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var rolePermission = await context.Set<RolePermissionEntity>()
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null) return false;

            context.Set<RolePermissionEntity>().Remove(rolePermission);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasPermissionAsync(Guid roleId, string permissionName)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<RolePermissionEntity>()
                .Include(rp => rp.Permission)
                .AnyAsync(rp => rp.RoleId == roleId && rp.Permission.Name == permissionName);
        }

        public async Task<IEnumerable<UserEntity>> GetUsersInRoleAsync(Guid roleId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<UserRoleEntity>()
                .Where(ur => ur.RoleId == roleId)
                .Include(ur => ur.User)
                .Select(ur => ur.User)
                .ToListAsync();
        }

        public async Task<bool> IsUserInRoleAsync(Guid userId, Guid roleId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<UserRoleEntity>()
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task<bool> AddUserToRoleAsync(Guid userId, Guid roleId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            // Check if user is already in the role
            var exists = await context.Set<UserRoleEntity>()
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (exists) return false;

            var userRole = new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            };

            context.Set<UserRoleEntity>().Add(userRole);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromRoleAsync(Guid userId, Guid roleId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var userRole = await context.Set<UserRoleEntity>()
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null) return false;

            context.Set<UserRoleEntity>().Remove(userRole);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
