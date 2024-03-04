using helper.v1.email.DTOs;

namespace helper.v1.email.Service
{
    public interface IEmailService
    {
        public Task SendEmail(SendEmailDTO body);
    }
}