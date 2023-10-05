namespace service.v1.email
{
    public interface IEmailService
    {
        public Task SendEmail(string emailTo, string msgTitle, string msgText);
    }
}