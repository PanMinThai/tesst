using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;

namespace TodoList_Project.Core.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserEntity, Guid>
    {
        Task<UserEntity> GetByIdWithIncludesAsync(Guid id);
        Task<UserEntity> GetByEmailAsync(string email);
        Task<bool> IsEmailConfirmedAsync(string email);
        Task LockUserAsync(Guid userId, DateTime lockEndTime);
        Task<int> GetFailedLoginAttemptsAsync(Guid userId);
    }
}
