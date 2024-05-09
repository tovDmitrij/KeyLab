using component.v1.email;

using helper.v1.configuration.Interfaces;

using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

namespace api.v1.email.Service
{
    public sealed class EmailService : IEmailService
    {
        private readonly SmtpClient _smptClient;
        private readonly MailboxAddress _admin;

        public EmailService(IEmailConfigurationHelper cfg)
        {
            _smptClient = new SmtpClient();

            var host = cfg.GetEmailHost();
            var port = cfg.GetEmailPort();
            _smptClient.Connect(host, port, SecureSocketOptions.StartTls);

            var login = cfg.GetEmailLogin();
            var password = cfg.GetEmailPassword();
            _smptClient.Authenticate(login, password);

            _admin = new MailboxAddress("KeyLab administration", login);
        }



        public async Task SendEmail(string emailTo, string msgTitle, string msgText)
        {
            var msg = new MimeMessage();
            msg.From.Add(_admin);
            msg.To.Add(new MailboxAddress("", emailTo));
            msg.Subject = msgTitle;
            msg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = msgText
            };

            await _smptClient.SendAsync(msg);
        }
    }
}