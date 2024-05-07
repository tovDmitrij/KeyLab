using component.v1.email;

namespace api.v1.email.Service
{
    public interface IEmailService
    {
        public Task SendEmail(string emailTo, string msgTitle, string msgText);
    }
}