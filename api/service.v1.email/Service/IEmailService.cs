using service.v1.email.DTOs;

namespace service.v1.email.Service
{
    public interface IEmailService
    {
        public Task SendEmail(SendEmailDTO body);
    }
}