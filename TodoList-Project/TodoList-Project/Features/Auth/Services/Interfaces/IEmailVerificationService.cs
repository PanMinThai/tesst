using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Services.Interfaces
{
    public interface IEmailVerificationService
    {
        string GenerateToken();
        Task SendConfirmationEmailAsync(string email, string token);
        bool ValidateToken(string email, string token);
    }

}
