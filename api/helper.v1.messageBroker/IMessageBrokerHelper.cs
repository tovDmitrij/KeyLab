namespace helper.v1.messageBroker
{
    public interface IMessageBrokerHelper
    {
        public Task PublishData<T>(T data);
    }
}