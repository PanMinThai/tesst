using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Entities.INI
{
    public class AuthConfig
    {
        public int MaxLoginAttempts { get; set; } = 5;
        public int AccountLockMinutes { get; set; } = 15;
        public int PasswordResetTokenExpiryHours { get; set; } = 24;
        public bool RequireConfirmedEmail { get; set; } = false;

        public JwtConfig Jwt { get; set; } = new JwtConfig();
    }
}
