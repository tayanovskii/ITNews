using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ITNews.Configurations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace ITNews.Services.Email
{
    public class EmailSender:IEmailSender
    {
        private readonly EmailSettings emailSettings;

        public EmailSender(IOptionsSnapshot<EmailSettings> options)
        {
            emailSettings = options.Value;
        }

        // Use our configuration to send the email by using SmtpClient
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(emailSettings.Host, emailSettings.Port)
            {
                Credentials = new NetworkCredential(emailSettings.UserEmail, emailSettings.Password),
                EnableSsl = emailSettings.EnableSSL
            };
            return client.SendMailAsync(
                new MailMessage(emailSettings.UserEmail, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}
