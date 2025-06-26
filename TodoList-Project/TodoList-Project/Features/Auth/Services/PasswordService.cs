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
            var bytes = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var saltedPassword = string.Concat(password, salt);
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(bytes);
        }

        public bool VerifyPassword(string password, string storedHash, string salt)
        {
            var hash = HashPassword(password, salt);
            return hash == storedHash;
        }
    }

}
