using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Models
{
    public class ChangePasswordModel
    {
        [Required] public string CurrentPassword { get; set; }
        [Required] public string NewPassword { get; set; }
    }
}
