using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_PhanMinhThai.Models
{
    public class UserModel
    {
            public Guid UserId { get; set; } = Guid.NewGuid();
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
