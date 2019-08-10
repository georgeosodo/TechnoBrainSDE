using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmployeeHierarchy.Domain
{
    public class Employees
    {
        public Employees(string[] csvInput)
        {
            if (csvInput == null || csvInput.Length==0) throw new ArgumentException(nameof(csvInput) + "Invalid CSV Input");
            employees = Employee.Create(csvInput);
            ValidateNumberOfCEOs();
            ValidateAllManagersAreListedAsEmployees();
            ValidateEmployeeWithMoreThanOneManger();
            ValidateCyclicReference();

        }

        public List<Employee> employees { get; set; }

        public Int32 GetManagersBudget(string managerId)
        {
            if (string.IsNullOrWhiteSpace(managerId)) throw new ArgumentException("Invalid Manager");
            Int32 total = 0;
            total += employees.FirstOrDefault(e => e.Id == managerId).Salary;
            foreach (var item in employees.Where(e => e.ManagerId == managerId))
            {
                if (isManager(item.Id))
                {
                    total += GetManagersBudget(item.Id);
                }
                else
                {
                    total += item.Salary;
                }
            }
            return Convert.ToInt32(total);
        }
        private bool isManager(string id) => employees.Where(e => e.ManagerId == id).Count() > 0;
               
        private void ValidateNumberOfCEOs()
        {
            if (employees.Where(e => e.ManagerId == string.Empty || e.ManagerId == null).Count() > 1)
            {
                throw new ArgumentException("More that one CEO Found");
                               
            }
        }

        private void ValidateAllManagersAreListedAsEmployees()
        {
            foreach (var _ in employees.Where(r => r.ManagerId != null && r.ManagerId != string.Empty)
                .Select(e => e.ManagerId).Where(manager => employees.FirstOrDefault(e => e.Id == manager) == null).Select(manager => new { }))
            {
                throw new ArgumentException("Manager not Listed as Employee");
            }
        }

        private void ValidateEmployeeWithMoreThanOneManger()
        {
            foreach (var id in employees.Select(e => e.Id).Distinct().Where(id => employees.Where(i => i.Id == id)
            .Select(m => m.ManagerId).Distinct().Count() > 1).Select(id => id))
            {
                throw new ArgumentException("Employee with more that one manager");
            }
        }

        private void ValidateCyclicReference()
        {
            foreach (var _ in from employee in employees.Where(e => e.ManagerId != string.Empty && e.ManagerId != null)
                              let manager = employees.Where(e => e.ManagerId != string.Empty && e.ManagerId != null)
                        .FirstOrDefault(e => e.Id == employee.ManagerId)
                              where manager != null
                              where manager.ManagerId == employee.Id
                              select new { })
            {
                throw new ArgumentException("Cyclic reference found");
            }
        }
    }
}
