using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Project.Core.DAL.Entities.INI
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public bool UseSsl { get; set; } = true;
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string BaseUrl { get; set; }
        public int EmailConfirmationExpiryHours { get; set; } = 24;
        public int PasswordResetExpiryMinutes { get; set; } = 30;
    }
}