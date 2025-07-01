using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Services
{
    public interface IPasswordResetService
    {
        Task<string> GenerateTokenAsync(Guid userId);
        Task<bool> SendResetEmailAsync(string email, string token);
        Task<bool> ValidateTokenAsync(string email, string token);
        Task<bool> MarkTokenAsUsedAsync(string email, string token);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }

}
