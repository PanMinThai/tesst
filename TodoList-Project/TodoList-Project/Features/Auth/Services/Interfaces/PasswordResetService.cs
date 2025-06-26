using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Services.Interfaces
{
    //public class PasswordResetService : IPasswordResetService
    //{
    //    private readonly Dictionary<string, string> _tokenStore = new(); // email -> token
    //    private readonly IEmailService _emailService;

    //    public PasswordResetService(IEmailService emailService)
    //    {
    //        _emailService = emailService;
    //    }

    //    public string GenerateToken()
    //    {
    //        return Guid.NewGuid().ToString();
    //    }

    //    public async Task SendResetEmailAsync(string email, string token)
    //    {
    //        _tokenStore[email] = token;

    //        var link = $"https://yourapp.com/reset-password?email={email}&token={token}";
    //        var message = $"Click để đặt lại mật khẩu: {link}";

    //        await _emailService.SendEmailAsync(email, "Reset mật khẩu", message);
    //    }

    //    public bool ValidateToken(string email, string token)
    //    {
    //        return _tokenStore.TryGetValue(email, out var storedToken) && storedToken == token;
    //    }

    //    public Task MarkTokenAsUsedAsync(string email, string token)
    //    {
    //        _tokenStore.Remove(email);
    //        return Task.CompletedTask;
    //    }
    //}

}
