using Microsoft.AspNetCore.Connections;
//using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Producer.rabbitmq
{
    public class RabbitProducer:IRabbitProducer
    {
        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq" };
            //var factory = new ConnectionFactory { HostName = "rabbitmq" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "money", durable: false, exclusive: false, autoDelete: false, arguments: null);
            //var json = JsonSerializer.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: "money", body: body);
        }
    }
}
