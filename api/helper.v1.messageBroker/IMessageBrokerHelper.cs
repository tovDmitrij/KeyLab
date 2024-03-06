namespace helper.v1.messageBroker
{
    public interface IMessageBrokerHelper
    {
        public Task SendData<T>(T data);
    }
}