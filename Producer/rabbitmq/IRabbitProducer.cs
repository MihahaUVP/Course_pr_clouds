namespace Producer.rabbitmq
{
    public interface IRabbitProducer
    {
        void SendMessage(string message);
    }
}
