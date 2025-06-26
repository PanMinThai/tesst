using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;

namespace TodoList_Project.Features.Auth.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(UserEntity user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
