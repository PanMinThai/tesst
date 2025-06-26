using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Features.Auth.Models
{
    public class RegisterModel
    {
        [Required][EmailAddress] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string DisplayName { get; set; }
    }
}
