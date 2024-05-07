using api.v1.email.Service;

using component.v1.email;

using MassTransit;

namespace api.v1.email.Consumers
{
    public sealed class EmailConsumer(IEmailService email, ILogger<EmailConsumer> logger) : IConsumer<SendEmailDTO>
    { 
        private readonly IEmailService _email = email;
        private readonly ILogger<EmailConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<SendEmailDTO> context)
        {
            var data = context.Message;
            _logger.LogInformation($">>>New msg: {data.EmailTo} - {data.MsgTitle} - {data.MsgText}");
            await _email.SendEmail(data.EmailTo, data.MsgTitle, data.MsgText);
        }
    }
}