using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Auth.Models;
using TodoList_Project.Features.Users.Models;

namespace TodoList_Project.Features.Auth.Services.Interfaces
{
    public interface ISessionService
    {
        Task<SessionModel> CreateSessionAsync(Guid userId);
        Task<bool> ValidateSessionAsync(string token);
        Task<UserModel> GetUserFromSessionAsync(string token);
        Task InvalidateSessionAsync(string token);
        Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId);
        Task ExtendSessionAsync(string token);
    }
}
