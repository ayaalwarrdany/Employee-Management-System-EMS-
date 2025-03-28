using EMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace Employee_Management_System
{
    internal class Company
    {
        public  List<Employee> employees { get; set; }
        public  List<Department> departments { get; set; }
        public Company()
        {
            employees = new List<Employee>();
           departments = new List<Department>();
        }



        public void SaveToFile(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                Console.WriteLine("JSON to save: " + json); // Debug: See what’s being serialized
                File.WriteAllText(filePath, json);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public static Company LoadFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    return JsonSerializer.Deserialize<Company>(json) ?? new Company();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
            return new Company(); // Return empty company if file fails
        }







        public void AddEmployee(Employee employee)
        {
            if (employees.Any(e => e.Id == employee.Id))
            {
                foreach(var emp in employees)
                {
                    Console.WriteLine($"id : {emp.Id} Name : {emp.Name}");
                }
                throw new Exception("Employee ID already exists.");
            }
            employees.Add(employee);
            var dept = departments.FirstOrDefault(d => d.Name == employee.DepartmentName);
            dept?.AddEmployee(employee);
        }
        public void AddDepartment(Department department)
        {
            if (!departments.Any(d => d.Name == department.Name))
                departments.Add(department);
        }
        //   Report Generation


        public void PromoteEmployee(int id, JobTitle newTitle, double newSalary)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee != null && employee.GetAveragePerformance() >= 4.0)
            {
                employee.Promote(newTitle, newSalary);
            }
            else
                throw new Exception("Employee not found or not eligible for promotion(rating < 4.0).");
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
        //         {
        //            Console.WriteLine($"Id : {emp.Id}  Name: {emp.Name} ({emp.Title})");
        //         }
        //    }
        //}
        public void GenerateDepartmentReport()
        {
            Console.WriteLine("\nDepartment Report:");

            // Department table header
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"| {"Department Name",-20} | {"Department Head",-35} |");
            Console.WriteLine(new string('-', 60));
            Console.ResetColor();

            foreach (var department in departments)
            {
                // Department information
                string deptHead = department.DepartmentHead?.Name ?? "None";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"| {department.Name,-20} | {deptHead,-35} |");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 60));
                Console.ResetColor();

                // Employee table header
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Employees in {department.Name}:");
                Console.WriteLine(new string('-', 90));
                Console.WriteLine($"| {"ID",-10} | {"Name",-25} | {"Title",-25} | {"Salary",-20} |");
                Console.WriteLine(new string('-', 90));

                // Employee details with alternating row colors
                bool alternateColor = false;
                foreach (var emp in department.GetEmployees())
                {
                    Console.ForegroundColor = alternateColor ? ConsoleColor.White : ConsoleColor.Gray;
                    Console.WriteLine($"| {emp.Id,-10} | {emp.Name,-25} | {emp.Title,-25} | {emp.Salary.ToString("C"),-20} |");
                    alternateColor = !alternateColor;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(new string('-', 90));
                Console.WriteLine(); // Empty line between departments
                Console.ResetColor();
            }
        }
        public void GenerateTopPerformersReport()
        {
            var topPerformers = employees.Where(e => !e.IsTerminated && e.GetAveragePerformance() >= 4.5).OrderByDescending(e => e.GetAveragePerformance());
            Console.WriteLine("\nTop Performers:");
            foreach (var emp in topPerformers)
                Console.WriteLine($"{emp.Name} - Avg Rating: {emp.GetAveragePerformance()}");
        }
        public void GenerateSalaryDistributionReport()
        {
            Console.WriteLine("\nSalary Distribution:");
            var activeEmployees = employees.Where(e =>!e.IsTerminated);
            Console.WriteLine($"Average Salary: {activeEmployees.Average(e => e.Salary)}");
            Console.WriteLine($"Highest Salary: {activeEmployees.Max(e => e.Salary)}");
            Console.WriteLine($"Lowest Salary: {activeEmployees.Min(e => e.Salary)}");
        }
        public void TransferEmployeeToDepartment(string departmentName , int employeeId)
        {

            var employee = employees.FirstOrDefault(e => e.Id ==employeeId);
            if (employee == null) throw new InvalidOperationException("Employee not found.");
            var oldDept = departments.FirstOrDefault(d => d.Name == employee.DepartmentName);
            var newDept = departments.FirstOrDefault(d => d.Name == departmentName);
            if (newDept == null) throw new InvalidOperationException("Target department not found.");
            oldDept?.RemoveEmployee(employee);
            newDept.AddEmployee(employee);

            //Department oldDepartment= 
            //Department.RemoveEmployee(this);
            //newDepartment.AddEmployee(this);
            //Department = newDepartment;

        }



    }
}
