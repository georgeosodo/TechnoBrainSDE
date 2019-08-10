using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeHierarchy.Domain
{
    public class Employee
    {
        public string Id { get; set; }
        public string ManagerId { get; set; }
        public Int16 Salary { get; set; }
        
        private Employee(string id, string managerId, Int16 salary)
        {         
           
            Id = id;
            ManagerId = managerId;
            Salary = salary;
        }
        public static List<Employee> Create(string[] CsvInput)
        {
            List<Employee> employees = new List<Employee>();
            foreach (var employeeInput in CsvInput )
            {
                var values = employeeInput.Split(',');
                Int16 EmployeeSalary;
                bool IsValidInteger = Int16.TryParse(values[2], out EmployeeSalary);
                if (!IsValidInteger || EmployeeSalary < 0) throw new ArgumentOutOfRangeException("Salary cannot be less that 0");                           
                employees.Add(new Employee(values[0], values[1], EmployeeSalary));
            }

            return employees;
        }
    }
}
