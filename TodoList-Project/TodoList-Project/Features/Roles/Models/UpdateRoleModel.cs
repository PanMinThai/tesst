using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Roles.Models
{
    public class UpdateRoleModel
    {
        [Required] public string Name { get; set; }
        public IEnumerable<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}
