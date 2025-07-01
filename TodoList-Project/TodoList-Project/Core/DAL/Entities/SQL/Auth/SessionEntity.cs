using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.Base;

namespace TodoList_Project.Core.DAL.Entities.SQL.Auth
{
    public class SessionEntity : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string IpAddress { get; set; }
        public string DeviceInfo { get; set; }
        public bool IsActive { get; set; }

        public UserEntity User { get; set; }
    }
}
