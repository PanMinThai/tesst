using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.INI;
using TodoList_Project.Features.Auth.Services.Interfaces;

namespace TodoList_Project.Features.Auth.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailConfig> emailConfig, ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderEmail));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                message.Body = new TextPart(isBodyHtml ? TextFormat.Html : TextFormat.Plain)
                {
                    Text = body
                };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    _emailConfig.SmtpServer,
                    _emailConfig.SmtpPort,
                    _emailConfig.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);

                if (!string.IsNullOrWhiteSpace(_emailConfig.SmtpUsername))
                {
                    await smtp.AuthenticateAsync(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword);
                }

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation($"Email sent to {to}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {to}");
                return false;
            }
        }

        public async Task<bool> SendConfirmationEmailAsync(string email, string token)
        {
            var confirmationLink = $"{_emailConfig.BaseUrl}/confirm-email?email={Uri.EscapeDataString(email)}&token={token}";

            var emailBody = $@"
            <h1>Xác nhận Email</h1>
            <p>Cảm ơn bạn đã đăng ký tài khoản. Vui lòng nhấp vào liên kết bên dưới để xác nhận địa chỉ email của bạn:</p>
            <p><a href='{confirmationLink}'>Xác nhận Email</a></p>
            <p>Nếu bạn không yêu cầu điều này, vui lòng bỏ qua email này.</p>
            <p>Liên kết này sẽ hết hạn sau {_emailConfig.EmailConfirmationExpiryHours} giờ.</p>";

            return await SendEmailAsync(
                to: email,
                subject: "Xác nhận địa chỉ email",
                body: emailBody);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string token)
        {
            var resetLink = $"{_emailConfig.BaseUrl}/reset-password?email={Uri.EscapeDataString(email)}&token={token}";

            var emailBody = $@"
            <h1>Đặt lại Mật khẩu</h1>
            <p>Bạn đã yêu cầu đặt lại mật khẩu. Vui lòng nhấp vào liên kết bên dưới để đặt lại mật khẩu của bạn:</p>
            <p><a href='{resetLink}'>Đặt lại Mật khẩu</a></p>
            <p>Nếu bạn không yêu cầu điều này, vui lòng bỏ qua email này.</p>
            <p>Liên kết này sẽ hết hạn sau {_emailConfig.PasswordResetExpiryMinutes} phút.</p>";

            return await SendEmailAsync(
                to: email,
                subject: "Yêu cầu đặt lại mật khẩu",
                body: emailBody);
        }

        public async Task<bool> SendPasswordChangedNotificationAsync(string email)
        {
            var emailBody = $@"
            <h1>Mật khẩu đã được thay đổi</h1>
            <p>Mật khẩu cho tài khoản của bạn vừa được thay đổi.</p>
            <p>Nếu bạn không thực hiện thay đổi này, vui lòng liên hệ với bộ phận hỗ trợ ngay lập tức.</p>";

            return await SendEmailAsync(
                to: email,
                subject: "Thông báo thay đổi mật khẩu",
                body: emailBody);
        }
    }
}
