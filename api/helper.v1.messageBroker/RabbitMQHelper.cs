using MassTransit;

namespace helper.v1.messageBroker
{
    public sealed class RabbitMQHelper(IBus bus) : IMessageBrokerHelper
    {
        private readonly IBus _bus = bus;

        public async Task PublishData<T>(T data) => await _bus.Publish(data!);
    }
}