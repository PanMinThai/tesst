using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.INI;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Core.DAL.Repositories.Interfaces;
using TodoList_Project.Features.Auth.Models;
using TodoList_Project.Features.Auth.Services.Interfaces;
using TodoList_Project.Features.Users.Models;

namespace TodoList_Project.Features.Auth.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly AuthConfig _authConfig;
        private readonly IMapper _mapper;

        public SessionService(
            ISessionRepository sessionRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IOptions<AuthConfig> authConfig,
            IMapper mapper)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _authConfig = authConfig.Value;
            _mapper = mapper;
        }

        public async Task<SessionModel> CreateSessionAsync(Guid userId)
        {
            var token = GenerateSessionToken();
            var expiresAt = DateTime.UtcNow.AddMinutes(_authConfig.SessionTimeoutMinutes);

            var session = new SessionEntity
            {
                Id = userId,
                Token = token,
                ExpiresAt = expiresAt,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _sessionRepository.AddAsync(session);

            return _mapper.Map<SessionModel>(session);
        }

        public async Task<bool> ValidateSessionAsync(string token)
        {
            var session = await _sessionRepository.GetByTokenAsync(token);
            return session != null;
        }

        public async Task<UserModel> GetUserFromSessionAsync(string token)
        {
            var session = await _sessionRepository.GetByTokenAsync(token);
            if (session == null) return null;

            var user = await _userRepository.GetByIdAsync(session.UserId);
            return _mapper.Map<UserModel>(user);
        }

        public async Task InvalidateSessionAsync(string token)
        {
            var session = await _sessionRepository.GetByTokenAsync(token);
            if (session != null)
            {
                await _sessionRepository.InvalidateSessionAsync(session.Id);
            }
        }

        public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return Enumerable.Empty<string>();

            var roles = user.UserRoles.Select(ur => ur.RoleId).ToList();
            var permissions = await _roleRepository.GetPermissionsForRolesAsync(roles);

            return permissions.Select(p => p.Name).Distinct().ToList();
        }

        public async Task ExtendSessionAsync(string token)
        {
            var session = await _sessionRepository.GetByTokenAsync(token);
            if (session != null)
            {
                session.ExpiresAt = DateTime.UtcNow.AddMinutes(_authConfig.SessionTimeoutMinutes);
                await _sessionRepository.UpdateAsync(session);
            }
        }

        private string GenerateSessionToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
