using MailKit.Net.Smtp;

using MimeKit;

namespace service.v1.email
{
    public sealed class EmailService : IEmailService
    {
        private readonly string adminEmail = "login@yandex.ru";
        private readonly string adminPassword = "password";

        
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", adminEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            var client = new SmtpClient();
            await client.ConnectAsync("smtp.yandex.ru", 25, false);
            await client.AuthenticateAsync(adminEmail, adminPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}