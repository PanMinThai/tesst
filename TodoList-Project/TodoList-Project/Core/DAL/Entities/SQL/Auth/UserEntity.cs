using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.Base;

namespace TodoList_Project.Core.DAL.Entities.SQL.Auth
{
    public class UserEntity :BaseEntity<Guid>
    {
        public string Email { get; set; } 
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Salt { get; set; }
        public string DisplayName { get; set; }
        public bool IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockedUntil { get; set; }
        public DateTime LastLogin { get; set; }
        public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
        public ICollection<LoginHistoryEntity> LoginHistories { get; set; } = new List<LoginHistoryEntity>();
        public ICollection<PasswordResetTokenEntity> PasswordResetTokens { get; set; } = new List<PasswordResetTokenEntity>();
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
