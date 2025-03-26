using EMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Employee_Management_System
{
    internal class Company
    {
        List<Employee> employees;
        List<Department> departments;
        public Company()
        {
            employees = new List<Employee>();
            departments = new List<Department>();
        }
        public void AddEmployee(Employee employee)
        {
            if (employees.Any(e => e.Id == employee.Id)) throw new Exception("Employee ID already exists.");
            employees.Add(employee);
            var dept = departments.FirstOrDefault(d => d.Name == employee.Department.Name);
            dept?.AddEmployee(employee);
        }
        public void AddDepartment(Department department)
        {
            if (!departments.Any(d => d.Name == department.Name))
                departments.Add(department);
        }
        // Report Generation
        //public void GenerateDepartmentReport()
        //{
        //    Console.WriteLine("\nDepartment Report:");
        //    foreach (var department in departments)
        //    {
        //        Console.WriteLine($"\n{department.Name} (Head: {department.DepartmentHead?.Name ?? "None"})");
        //        Console.WriteLine($"Employees: {department.GetEmployees().Count}");
        //        foreach (var emp in department.GetEmployees())
        //        {
        //            Console.WriteLine($"- {emp.Name} ({emp.JobTitle})");
        //        }
        //    }
        //}
    //    public void GenerateTopPerformersReport()
    //    {
    //        Console.WriteLine("\nTop Performers: ");
    //        var topPerformers = employees.Where(e => e.IsActive)
    //            .OrderByDescending(e => e.GetAveragePerformance()).Take(3);
    //        foreach(var emp in topPerformers)
    //        {
    //            Console.WriteLine($"{emp.Name} - Avg Rating: {emp.GetAveragePerformance()}");
    //        }
    //    }
    //    public void GenerateSalaryDistributionReport()
    //    {
    //        Console.WriteLine("\nSalary Distribution:");
    //        var activeEmployees = employees.Where(e => e.IsActive);
    //        Console.WriteLine($"Average Salary: {activeEmployees.Average(e => e.Salary)}");
    //        Console.WriteLine($"Highest Salary: {activeEmployees.Max(e => e.Salary)}");
    //        Console.WriteLine($"Lowest Salary: {activeEmployees.Min(e => e.Salary)}");
    //    }
    }
}
