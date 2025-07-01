using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Core.DAL.Repositories.Interfaces;

namespace TodoList_Project.Core.DAL.Repositories.Auth
{
    public class UserRepository : GenericRepository<UserEntity, Guid>, IUserRepository
    {
        public UserRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<UserEntity>()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<int> GetFailedLoginAttemptsAsync(Guid userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var user = await context.Set<UserEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.FailedLoginAttempts ?? 0;
        }

        public async Task<bool> IsEmailConfirmedAsync(string email)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<UserEntity>()
                .AsNoTracking()
                .AnyAsync(u => u.Email == email && u.EmailConfirmed);
        }

        public async Task LockUserAsync(Guid userId, DateTime lockEndTime)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var user = await context.Set<UserEntity>().FindAsync(userId);
            if (user != null)
            {
                user.IsLocked = true;
                user.LockedUntil = lockEndTime;
                await context.SaveChangesAsync();
            }
        }
        public async Task<UserEntity> GetByIdWithIncludesAsync(Guid id)
        {
            await using var context = _contextFactory.CreateDbContext();

            return await context.Users
                .Where(u => u.Id == id)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync();
        }

    }
}