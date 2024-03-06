using MassTransit;

namespace helper.v1.messageBroker
{
    public sealed class RabbitMQHelper : IMessageBrokerHelper
    {
        private readonly IBus _bus;

        public RabbitMQHelper(IBus bus) => _bus = bus;



        public async Task SendData<T>(T data)
        {
            await _bus.Publish(data);
        }
    }
}