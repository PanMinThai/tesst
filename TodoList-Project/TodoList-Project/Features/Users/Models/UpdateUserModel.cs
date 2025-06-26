using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Users.Models
{
    public class UpdateUserModel
    {
        public string DisplayName { get; set; }
        public bool? IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
    }
}
