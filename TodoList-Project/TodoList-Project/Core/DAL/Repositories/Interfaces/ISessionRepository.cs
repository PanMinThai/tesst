using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<SessionEntity, Guid>
    {
        Task<SessionEntity> GetByTokenAsync(string token);
        Task InvalidateSessionAsync(Guid sessionId);
        Task InvalidateAllSessionsForUserAsync(Guid userId);
        Task<bool> IsValidSessionAsync(Guid sessionId);
    }
}
