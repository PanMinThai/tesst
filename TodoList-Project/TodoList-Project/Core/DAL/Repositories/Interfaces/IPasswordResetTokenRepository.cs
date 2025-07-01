using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public interface IPasswordResetTokenRepository : IGenericRepository<PasswordResetTokenEntity, Guid>
    {
        Task<PasswordResetTokenEntity> GetValidTokenAsync(Guid userId, string token);
        Task InvalidateUserTokensAsync(Guid userId);
        Task CleanUpExpiredTokensAsync();
    }
}
