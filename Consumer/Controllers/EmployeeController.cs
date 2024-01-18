using Consumer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private IList<Employee> _employees { get; set; }
        public EmployeeController()
        {
            _employees = Employees.Workers;
        }
        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(_employees);
        }
    }
}
