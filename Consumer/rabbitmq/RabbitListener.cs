using Consumer.Models;
using Consumer.Services;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Consumer.rabbitmq
{
    public class RabbitListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitListener(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq" };
            //var factory = new ConnectionFactory { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "money", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _serviceProvider = serviceProvider;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Сообщение получено: {content}");
                //var order = JsonSerializer.Deserialize<Employee>(content) ?? throw new Exception();
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ISalaryService>();
                ///В очередь должны приходить числа
                int id = Convert.ToInt32(content);
                service.IncreaseSalary(id);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("money", false, consumer);

            return Task.CompletedTask;
        }
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
