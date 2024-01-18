using Confluent.Kafka;
using Consumer.Services;
using Microsoft.OpenApi.Validations;
using System.Text.Json;

namespace Consumer.kafka
{
    public class KafkaListener : BackgroundService
    {
        const string topic = "salary-kafka";
        const string groupId = "test_group";
        const string bootstrapServers = "kafka:9092";

        private readonly IServiceProvider _serviceProvider;


        ConsumerConfig config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        public KafkaListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => ActivateConsumer(stoppingToken));
            return Task.CompletedTask;
        }
        private async Task ActivateConsumer(CancellationToken stoppingToken)
        {
            try
            {
                using var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
                consumerBuilder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancelToken.Token);
                        // var number = JsonSerializer.Deserialize<string>(consumer.Message.Value) ?? throw new Exception(); ;
                        //var number = Convert.ToInt32(consumer.Message.Value);
                        Console.WriteLine("Kafka: сообщение получено " + consumer.Message.Value);
                        using var scope = _serviceProvider.CreateScope();
                        var service = scope.ServiceProvider.GetRequiredService<ISalaryService>();
                        //Console.WriteLine("Сообщение, что всё хорошо");
                        service.IncreaseSalary(Convert.ToInt32(consumer.Message.Value));
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
