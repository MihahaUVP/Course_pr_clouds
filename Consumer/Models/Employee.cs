namespace Consumer.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary {  get; set; }
    }

    public static class Employees
    {
        public static IList<Employee> Workers = new List<Employee>
            {
                new Employee{Id = 1, Name = "Misha", Salary = 1000},
                new Employee{Id = 2, Name = "Ashim", Salary = 1500},
                new Employee{Id = 3, Name = "Mikhail", Salary = 2000}
            };
    }
}
