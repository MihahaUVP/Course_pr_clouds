using Microsoft.AspNetCore.Mvc;
using Producer.Models;
using Producer.rabbitmq;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IRabbitProducer _messagePublisher;
        public EmployeeController(IRabbitProducer messagePublisher)
        {
            //_order = new List<Order>();
            _messagePublisher = messagePublisher;
        }
        [HttpPost]
        public IActionResult CreateSalary(string number)
        {          
            _messagePublisher.SendMessage(number);
            return Ok();
        }
    }
}
