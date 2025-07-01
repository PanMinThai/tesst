using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Core.DAL.Repositories.Interfaces;

namespace TodoList_Project.Core.DAL.Repositories.Auth
{
    public class PasswordResetTokenRepository : GenericRepository<PasswordResetTokenEntity, Guid>, IPasswordResetTokenRepository
    {
        public PasswordResetTokenRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<PasswordResetTokenEntity> GetValidTokenAsync(Guid userId, string token)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<PasswordResetTokenEntity>()
                .FirstOrDefaultAsync(t =>
                    t.UserId == userId &&
                    t.Token == token &&
                    !t.IsUsed &&
                    t.ExpiresAt > DateTime.UtcNow);
        }

        public async Task InvalidateUserTokensAsync(Guid userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var tokens = await context.Set<PasswordResetTokenEntity>()
                .Where(t => t.UserId == userId && !t.IsUsed)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsUsed = true;
            }

            await context.SaveChangesAsync();
        }

        public async Task CleanUpExpiredTokensAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var expiredTokens = await context.Set<PasswordResetTokenEntity>()
                .Where(t => t.ExpiresAt <= DateTime.UtcNow || t.IsUsed)
                .ToListAsync();

            context.Set<PasswordResetTokenEntity>().RemoveRange(expiredTokens);
            await context.SaveChangesAsync();
        }
    }
}
