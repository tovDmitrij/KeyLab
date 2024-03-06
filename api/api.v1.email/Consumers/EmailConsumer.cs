using api.v1.email.Service;

using component.v1.email;

using MassTransit;

namespace api.v1.email.Consumers
{
    public sealed class EmailConsumer : IConsumer<SendEmailDTO>
    { 
        private readonly IEmailService _email;

        public EmailConsumer(IEmailService email, ILogger<EmailConsumer> logger) => _email = email;



        public async Task Consume(ConsumeContext<SendEmailDTO> context)
        {
            var data = context.Message;
            await _email.SendEmail(data);
        }
    }
}