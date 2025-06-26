using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Auth.Services.Interfaces;

namespace TodoList_Project.Features.Auth.Services
{
    //public class EmailVerificationService : IEmailVerificationService
    //{
    //    private readonly Dictionary<string, string> _tokenStore = new(); // email -> token
    //    private readonly IEmailService _emailService;

    //    public EmailVerificationService(IEmailService emailService)
    //    {
    //        _emailService = emailService;
    //    }

    //    public string GenerateToken()
    //    {
    //        return Guid.NewGuid().ToString();
    //    }

    //    public async Task SendConfirmationEmailAsync(string email, string token)
    //    {
    //        _tokenStore[email] = token;

    //        var link = $"https://yourapp.com/confirm-email?email={email}&token={token}";
    //        var message = $"Click vào đây để xác thực email: {link}";

    //        await _emailService.SendEmailAsync(email, "Xác thực email", message);
    //    }

    //    public bool ValidateToken(string email, string token)
    //    {
    //        return _tokenStore.TryGetValue(email, out var storedToken) && storedToken == token;
    //    }
    //}

}
