using component.v1.email;

namespace api.v1.email.Service
{
    public interface IEmailService
    {
        public Task SendEmail(SendEmailDTO body);
    }
}