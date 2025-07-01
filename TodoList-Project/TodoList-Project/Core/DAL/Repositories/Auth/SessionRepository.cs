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
    public class SessionRepository : GenericRepository<SessionEntity, Guid>, ISessionRepository
    {
        public SessionRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<SessionEntity> GetByTokenAsync(string token)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<SessionEntity>()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Token == token && s.IsActive && s.ExpiresAt > DateTime.UtcNow);
        }

        public async Task InvalidateSessionAsync(Guid sessionId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var session = await context.Set<SessionEntity>().FindAsync(sessionId);
            if (session != null)
            {
                session.IsActive = false;
                await context.SaveChangesAsync();
            }
        }

        public async Task InvalidateAllSessionsForUserAsync(Guid userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var sessions = await context.Set<SessionEntity>()
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive = false;
            }

            await context.SaveChangesAsync();
        }

        public async Task<bool> IsValidSessionAsync(Guid sessionId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<SessionEntity>()
                .AnyAsync(s => s.Id == sessionId && s.IsActive && s.ExpiresAt > DateTime.UtcNow);
        }
    }
}
