using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Services
{
    public interface IPasswordResetService
    {
        string GenerateToken();
        Task SendResetEmailAsync(string email, string token);
        bool ValidateToken(string email, string token);
        Task MarkTokenAsUsedAsync(string email, string token);
    }

}
