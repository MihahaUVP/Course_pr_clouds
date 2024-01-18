using Consumer.Models;

namespace Consumer.Services
{
    public class SalaryService:ISalaryService
    {
        IList<Employee> _employes;
        public SalaryService() 
        {
            _employes = Employees.Workers.ToList();
        }
        /// <summary>
        /// Увеличение зарплаты конкретного работника на 10%
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void IncreaseSalary(int id)
        {
            var _employee = _employes.Where(x => x.Id == id).FirstOrDefault() ?? throw new Exception("Пользователь не найден");
            _employee.Salary = (int)(_employee.Salary*1.1);
        }
    }
}
