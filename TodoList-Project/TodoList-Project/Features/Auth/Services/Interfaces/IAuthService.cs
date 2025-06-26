using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Auth.Models;

namespace TodoList_Project.Features.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultModel> RegisterAsync(RegisterModel model);
        Task<AuthResultModel> LoginAsync(LoginModel model);
        Task<AuthResultModel> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> ConfirmEmailAsync(string email, string token);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordModel model);
        Task<bool> LogoutAsync(Guid userId);
    }
}
