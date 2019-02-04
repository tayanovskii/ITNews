using System.Threading.Tasks;
using ITNews.Configurations;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ITNews.Services.Email
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptionsSnapshot<EmailSettings> options)
        {
            emailSettings = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(emailSettings.UserEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s,c,h,e) => true;
                await client.ConnectAsync(emailSettings.Host, emailSettings.Port, emailSettings.EnableSSL);
                await client.AuthenticateAsync(emailSettings.UserEmail, emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
