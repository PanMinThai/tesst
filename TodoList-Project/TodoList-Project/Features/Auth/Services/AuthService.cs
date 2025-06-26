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
using TodoList_Project.Features.Users.Models;

namespace TodoList_Project.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        //private readonly IEmailVerificationService _emailVerificationService;
        private readonly IPasswordResetService _passwordResetService;
        private readonly AuthConfig _authConfig;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IPasswordService passwordService,

            
            
            IOptions<AuthConfig> authConfig,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordService = passwordService;
            
            
            _authConfig = authConfig.Value;
            _mapper = mapper;
        }

        public async Task<AuthResultModel> RegisterAsync(RegisterModel model)
        {
            if (await _userRepository.ExistsAsync(u => u.Email == model.Email))
                throw new AppException("Email đã được sử dụng");

            var salt = _passwordService.GenerateSalt();
            var hash = _passwordService.HashPassword(model.Password, salt);

            var user = new UserEntity
            {
                Email = model.Email,
                PasswordHash = hash,
                Salt = salt,
                DisplayName = model.DisplayName,
                EmailConfirmed = !_authConfig.RequireConfirmedEmail,
                CreatedAt = DateTime.UtcNow
            };

            //await _userRepository.AddAsync(user);

            //if (_authConfig.RequireConfirmedEmail)
            //{
            //    var token = _emailVerificationService.GenerateToken();
            //    await _emailVerificationService.SendConfirmationEmailAsync(user.Email, token);
            //}

            return BuildAuthResult(user);
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

            user.FailedLoginAttempts = 0;
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return BuildAuthResult(user);
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            //var user = await _userRepository.GetByEmailAsync(email);
            //if (user == null) return false;

            //if (!_emailVerificationService.ValidateToken(email, token)) return false;

            //user.EmailConfirmed = true;
            //await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return true;

            var token = _passwordResetService.GenerateToken();
            await _passwordResetService.SendResetEmailAsync(email, token);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null) return false;

            if (!_passwordResetService.ValidateToken(model.Email, model.Token)) return false;

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

        public async Task<AuthResultModel> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            var identity = principal.Identity as ClaimsIdentity;
            var userId = Guid.Parse(identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new SecurityTokenException("Token không hợp lệ"));

            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new SecurityTokenException("Invalid token");

            return BuildAuthResult(user);
        }

        public Task<bool> LogoutAsync(Guid userId) => Task.FromResult(true);

        private AuthResultModel BuildAuthResult(UserEntity user)
        {
            var token = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResultModel
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_authConfig.Jwt.ExpiryMinutes),
                User = _mapper.Map<UserModel>(user)
            };
        }
    }

}
