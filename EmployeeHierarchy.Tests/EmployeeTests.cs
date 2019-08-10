using EmployeeHierarchy.Domain;
using System;
using Xunit;

namespace EmployeeHierarchy.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void EmployeesShouldThrowExceptionWhenCsvStringNullOrEmpty()
        {
            
            string[] CsvInput= { };             
            Assert.Throws<ArgumentException>(() => new Employees(CsvInput));
        }
        [Fact]
        public void SalariesShouldBeValidIntegers()
        {
            string employee1 = "Employee2,Employee1,-2";
            string employee2 = "Employee2,Employee1,-500";
            string[] CsvInput= { employee1, employee2 };
            Assert.Throws<ArgumentOutOfRangeException>(() => Employee.Create(CsvInput));
        }

        [Fact]
        public void EmployeesShouldHaveOnlyOneCEO()
        {
            string employee1 = "Employee2,,200";
            string employee2 = "Employee1,,500";
            string[] CsvInput = { employee1, employee2 };
            Assert.Throws<ArgumentException>(() => new Employees(CsvInput));

        }

        [Fact]
        public void ManagerShouldBeListedAsEmployee()
        {
            string employee1 = "Employee2,Employee3,200";
            string employee2 = "Employee1,Employee4,500";
            string[] CsvInput = { employee1, employee2 };
            Assert.Throws<ArgumentException>(() => new Employees(CsvInput));
        }

        [Fact]
        public void EmployeeShouldOnlyHaveOneManager()
        {
            string employee1 = "Employee2,Employee3,200";
            string employee2 = "Employee2,Employee4,500";
            string[] CsvInput = { employee1, employee2 };
            Assert.Throws<ArgumentException>(() => new Employees(CsvInput));
        }

        [Fact]
        public void EmployeeShouldNotHaveCyclicReference()
        {
            string employee1 = "Employee2,Employee3,200";
            string employee2 = "Employee3,Employee2,500";
            string[] CsvInput = { employee1, employee2 };
            Assert.Throws<ArgumentException>(() => new Employees(CsvInput));
        }

        [Fact]
        public void CalculatesCorrectManagerBudget()
        {
            string employee1 = "Employee2,,800";
            string employee2 = "Employee1,Employee2,500";
            string[] CsvInput = { employee1, employee2 };
            Int32 ExpectedBudget=1300;
            Int32 TotalBudget;
            Employees employees = new Employees(CsvInput);
            TotalBudget = employees.GetManagersBudget("Employee2");
            Assert.Equal(ExpectedBudget,TotalBudget);            
        }


    }
}
