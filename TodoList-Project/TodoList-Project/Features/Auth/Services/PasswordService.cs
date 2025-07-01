using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Services
{
    public class PasswordService : IPasswordService
    {
        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool VerifyPassword(string inputPassword, string storedHash, string storedSalt = null)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }
    }
}


