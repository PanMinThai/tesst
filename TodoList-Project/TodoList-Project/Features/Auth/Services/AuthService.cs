using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps;
using TodoList_Project.Core.DAL.Entities.INI;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Core.DAL.Repositories.Interfaces;
using TodoList_Project.Core.Utils.Exceptions;
using TodoList_Project.Features.Auth.Models;
using TodoList_Project.Features.Auth.Services.Interfaces;
using TodoList_Project.Features.Roles.Models;
using TodoList_Project.Features.Users.Models;

namespace TodoList_Project.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ISessionService _sessionService;
        private readonly IEmailService _emailService;   
        private readonly IPasswordResetService _passwordResetService;
        private readonly AuthConfig _authConfig;
        private readonly IMapper _mapper;
        private readonly ILoginHistoryService _loginHistoryService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IPasswordResetService passwordResetService,
            IOptions<AuthConfig> authConfig,
            IMapper mapper,
            ILoginHistoryService loginHistoryService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _passwordResetService = passwordResetService;
            _authConfig = authConfig.Value;
            _mapper = mapper;
            _loginHistoryService = loginHistoryService;
        }

        public async Task<AuthResultModel> LoginAsync(LoginModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email)
                       ?? throw new UnauthorizedException("Email hoặc mật khẩu không đúng");

            if (user.IsLocked && user.LockedUntil > DateTime.UtcNow)
                throw new AccountLockedException($"Tài khoản bị khóa đến {user.LockedUntil.Value.ToLocalTime()}");

            if (!_passwordService.VerifyPassword(model.Password, user.PasswordHash, user.Salt))
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= _authConfig.MaxLoginAttempts)
                {
                    user.IsLocked = true;
                    user.LockedUntil = DateTime.UtcNow.AddMinutes(_authConfig.AccountLockMinutes);
                }

                await _userRepository.UpdateAsync(user);
                throw new UnauthorizedException("Email hoặc mật khẩu không đúng");
            }

            if (_authConfig.RequireConfirmedEmail && !user.EmailConfirmed)
                throw new UnauthorizedException("Vui lòng xác thực email trước khi đăng nhập");

            // Reset failed attempts and update last login
            user.FailedLoginAttempts = 0;
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            // Create session
            var session = await _sessionService.CreateSessionAsync(user.Id);

            // Get user permissions
            var permissions = await _sessionService.GetUserPermissionsAsync(user.Id);

            return new AuthResultModel
            {
                Session = session,
                User = _mapper.Map<UserModel>(user),
                Permissions = permissions
            };
        }

        // Trong AuthService của bạn
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return true; // Trả về true để không tiết lộ email có tồn tại

            string token = await _passwordResetService.GenerateTokenAsync(user.Id);
            await _emailService.SendPasswordResetEmailAsync(email, token); // Gửi email
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null) return false;
            
            if (!await _passwordResetService.ValidateTokenAsync(model.Email, model.Token)) return false;

            var salt = _passwordService.GenerateSalt();
            user.PasswordHash = _passwordService.HashPassword(model.NewPassword, salt);
            user.Salt = salt;

            await _userRepository.UpdateAsync(user);
            await _passwordResetService.MarkTokenAsUsedAsync(model.Email, model.Token);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordModel model)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            if (!_passwordService.VerifyPassword(model.CurrentPassword, user.PasswordHash, user.Salt))
                return false;

            var salt = _passwordService.GenerateSalt();
            user.PasswordHash = _passwordService.HashPassword(model.NewPassword, salt);
            user.Salt = salt;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<UserPermissionsModel> GetUserPermissionsAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdWithIncludesAsync(userId);

            if (user == null) return null;

            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToList();

            return new UserPermissionsModel
            {
                UserId = userId,
                Permissions = permissions
            };
        }
    }

}
