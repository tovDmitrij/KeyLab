using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;
using service.v1.configuration.Interfaces;
using service.v1.email.DTOs;

namespace service.v1.email.Service
{
    public sealed class EmailService : IEmailService
    {
        private readonly SmtpClient _smptClient;
        private readonly MailboxAddress _admin;

        public EmailService(IEmailConfigurationService cfg)
        {
            _smptClient = new SmtpClient();

            var host = cfg.GetEmailHost();
            var port = cfg.GetEmailPort();
            _smptClient.Connect(host, port, SecureSocketOptions.StartTls);

            var login = cfg.GetEmailLogin();
            var password = cfg.GetEmailPassword();
            _smptClient.Authenticate(login, password);

            _admin = new MailboxAddress("Keyboard administration", login);
        }



        public async Task SendEmail(SendEmailDTO body)
        {
            var msg = new MimeMessage();
            msg.From.Add(_admin);
            msg.To.Add(new MailboxAddress("", body.EmailTo));
            msg.Subject = body.MsgTitle;
            msg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body.MsgText
            };

            await _smptClient.SendAsync(msg);
        }
    }
}