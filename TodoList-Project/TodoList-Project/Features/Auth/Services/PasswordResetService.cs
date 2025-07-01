using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.DBContext;
using TodoList_Project.Core.DAL.Entities.INI;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Core.DAL.Repositories.Interfaces;
using TodoList_Project.Features.Auth.Services.Interfaces;

namespace TodoList_Project.Features.Auth.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly AuthConfig _authConfig;

        public PasswordResetService(
            IUserRepository userRepository,
            IPasswordResetTokenRepository tokenRepository,
            IPasswordService passwordService,
            IEmailService emailService,
            IOptions<AuthConfig> authConfig)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _passwordService = passwordService;
            _emailService = emailService;
            _authConfig = authConfig.Value;
        }

        public async Task<string> GenerateTokenAsync(Guid userId)
        {
            // Generate a secure random token
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            // Create and save the token
            var resetToken = new PasswordResetTokenEntity
            {
                UserId = userId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_authConfig.PasswordResetTokenExpiryMinutes),
                IsUsed = false
            };

            await _tokenRepository.AddAsync(resetToken);

            return token;
        }

        public async Task<bool> SendResetEmailAsync(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return true; // Return true to avoid email enumeration attacks

            // In a real application, you would generate a proper reset link
            var resetLink = $"https://yourapp.com/reset-password?email={Uri.EscapeDataString(email)}&token={token}";

            var emailBody = $@"
            <h1>Password Reset Request</h1>
            <p>You requested to reset your password. Please click the link below to proceed:</p>
            <p><a href='{resetLink}'>{resetLink}</a></p>
            <p>If you didn't request this, please ignore this email.</p>
            <p>This link will expire in {_authConfig.PasswordResetTokenExpiryMinutes} minutes.</p>";

            return await _emailService.SendEmailAsync(
                to: email,
                subject: "Password Reset Request",
                body: emailBody);
        }

        public async Task<bool> ValidateTokenAsync(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;

            var resetToken = await _tokenRepository.GetValidTokenAsync(user.Id, token);
            return resetToken != null &&
                   !resetToken.IsUsed &&
                   resetToken.ExpiresAt > DateTime.UtcNow;
        }

        public async Task<bool> MarkTokenAsUsedAsync(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;

            var resetToken = await _tokenRepository.GetValidTokenAsync(user.Id, token);
            if (resetToken == null) return false;

            resetToken.IsUsed = true;
            await _tokenRepository.UpdateAsync(resetToken);

            // Invalidate all other tokens for this user
            await _tokenRepository.InvalidateUserTokensAsync(user.Id);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            // Validate token first
            if (!await ValidateTokenAsync(email, token))
                return false;

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;

            // Update password
            var salt = _passwordService.GenerateSalt();
            user.PasswordHash = _passwordService.HashPassword(newPassword, salt);
            user.Salt = salt;

            await _userRepository.UpdateAsync(user);

            // Mark token as used
            await MarkTokenAsUsedAsync(email, token);

            return true;
        }
    }
}
