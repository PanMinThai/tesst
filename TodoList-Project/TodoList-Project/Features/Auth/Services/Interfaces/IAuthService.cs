using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Auth.Models;
using TodoList_Project.Features.Roles.Models;

namespace TodoList_Project.Features.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultModel> LoginAsync(LoginModel model);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordModel model);
        Task<UserPermissionsModel> GetUserPermissionsAsync(Guid userId);
    }
}
