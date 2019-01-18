using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace ITNews.Services
{
    public class EmailService : IEmailSender
    {
        private readonly string host;
        private readonly int port;
        private readonly bool enableSSL;
        private readonly string userEmail;
        private readonly string password;

        public EmailService(string host, int port, bool enableSSL, string userEmail, string password)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.userEmail = userEmail;
            this.password = password;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(userEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(host, port, enableSSL);
                await client.AuthenticateAsync(userEmail, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
