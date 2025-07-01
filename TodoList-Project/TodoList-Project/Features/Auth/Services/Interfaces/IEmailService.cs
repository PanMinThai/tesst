using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true);
        Task<bool> SendConfirmationEmailAsync(string email, string token);
        Task<bool> SendPasswordResetEmailAsync(string email, string token);
        Task<bool> SendPasswordChangedNotificationAsync(string email);
    }
}
