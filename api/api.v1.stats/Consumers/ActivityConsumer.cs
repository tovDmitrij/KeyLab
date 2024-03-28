using api.v1.stats.Services.History;

using component.v1.activity;

using MassTransit;

namespace api.v1.stats.Consumers
{
    public sealed class ActivityConsumer(ILogger<ActivityConsumer> logger, IHistoryService history) : IConsumer<ActivityDTO>
    {
        private readonly ILogger<ActivityConsumer> _logger = logger;
        private readonly IHistoryService _history = history;

        public async Task Consume(ConsumeContext<ActivityDTO> context)
        {
            var data = context.Message;
            _logger.LogInformation($">>>New msg: {data.UserID} - {data.ActivityTag}");
            _history.AddActivityInHistory(data);
        }
    }
}