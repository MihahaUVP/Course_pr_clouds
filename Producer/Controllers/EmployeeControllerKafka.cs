using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeControllerKafka : Controller
    {
        private readonly string bootstrapServers = "kafka:9092";
        private readonly string topic = "salary-kafka";


        [HttpPost("api/kafka")]
        public async Task<IActionResult> Post(string request)
        {
            //var message = JsonSerializer.Serialize(request);
            return Ok(await SendRequest(topic, request));
        }

        private async Task<bool> SendRequest(string topic, string message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using var producer = new ProducerBuilder<Null, string>(config).Build();

                var result = await producer.ProduceAsync(topic, new Message<Null, string>
                {
                    Value = message
                });
                Console.WriteLine(message);
                Console.WriteLine(result.Timestamp.UtcDateTime);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}
