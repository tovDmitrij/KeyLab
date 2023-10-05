using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

using service.v1.configuration;

namespace service.v1.email
{
    public sealed class EmailService : IEmailService
    {
        private readonly SmtpClient _smpt;
        private readonly MailboxAddress _admin;
        private readonly IConfigurationService _cfg;

        public EmailService(IConfigurationService cfg)
        {
            _cfg = cfg;
            _smpt = new SmtpClient();

            var host = _cfg.GetEmailHost();
            var port = _cfg.GetEmailPort();
            _smpt.Connect(host, port, SecureSocketOptions.StartTls);

            var login = _cfg.GetEmailLogin();
            var password = _cfg.GetEmailPassword();
            _smpt.Authenticate(login, password);

            _admin = new MailboxAddress("Keyboard administration", login);
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

            await _smpt.SendAsync(msg);
        }
    }
}