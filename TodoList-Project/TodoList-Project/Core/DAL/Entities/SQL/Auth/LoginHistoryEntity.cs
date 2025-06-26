using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Entities.SQL.Auth
{
    public class LoginHistoryEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; }
        public string DeviceInfo { get; set; }
        public UserEntity User { get; set; }
    }
}
