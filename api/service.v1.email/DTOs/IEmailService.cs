using service.v1.email.Service;

namespace service.v1.email.DTOs
{
    public interface IEmailService
    {
        public Task SendEmail(SendEmailDTO body);
    }
}