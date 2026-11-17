using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace Cinema.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // The Gmail App Password is read from an environment variable (never hard-code secrets).
            var appPassword = Environment.GetEnvironmentVariable("EMAIL_APP_PASSWORD");

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("samirmohammedhamed@gmail.com", appPassword)
            };

            var mailMessage = new MailMessage(
                from: "samirmohammedhamed@gmail.com",
                to: email,
                subject,
                htmlMessage)
            {
                IsBodyHtml = true
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}
