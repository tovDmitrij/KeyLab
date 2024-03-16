using component.v1.preview;

using helper.v1.file;

using MassTransit;

namespace api.v1.main.Consumers
{
    public sealed class CompletePreviewConsumer : IConsumer<PreviewDTO>
    {
        private readonly ILogger<CompletePreviewConsumer> _logger;
        private readonly IFileHelper _file;

        public CompletePreviewConsumer(ILogger<CompletePreviewConsumer> logger, IFileHelper file)
        {
            _logger = logger;
            _file = file;
        }



        public async Task Consume(ConsumeContext<PreviewDTO> context)
        {
            var data = context.Message;
            _logger.LogInformation($">>>New msg: {data.FilePath}");

            var bytes = Convert.FromBase64String(data.File);
            _file.AddFile(bytes, data.FilePath);
        }
    }
}