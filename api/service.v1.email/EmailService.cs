using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

using service.v1.configuration;

namespace service.v1.email
{
    public sealed class EmailService : IEmailService, IDisposable
    {
        private readonly ISmtpClient _smptClient;
        private readonly MailboxAddress _admin;

        public EmailService(IConfigurationService cfg)
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



        public void Dispose()
        {
            _smptClient.Disconnect(true);
            _smptClient.Dispose();
        }
    }
}