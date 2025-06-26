using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Features.Users.Models;

namespace TodoList_Project.Features.Auth.Models
{
    public class AuthResultModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public UserModel User { get; set; }
    }
}
