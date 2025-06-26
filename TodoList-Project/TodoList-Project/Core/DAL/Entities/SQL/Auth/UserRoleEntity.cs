using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Entities.SQL.Auth
{
    public class UserRoleEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public UserEntity User { get; set; }
        public RoleEntity Role { get; set; }
    }
}
