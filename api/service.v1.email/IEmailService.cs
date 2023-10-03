namespace service.v1.email
{
    public interface IEmailService
    {
        /// <param name="email">Кому отправляем</param>
        /// <param name="subject">Тема письма</param>
        /// <param name="message">Текст письма</param>
        public Task SendEmailAsync(string email, string subject, string message);
    }
}