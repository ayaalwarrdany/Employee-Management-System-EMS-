                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              using EMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace EMS
{
    internal class Company
    {
        public List<Employee> employees { get; set; }
        public List<Department> departments { get; set; }
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
            // Console.WriteLine($"Debug: Attempting to load file from {filePath}");
            try
            {
                if (File.Exists(filePath))
                {
                    // Console.WriteLine("Debug: File exists, reading content...");
                    string json = File.ReadAllText(filePath);
                    // Console.WriteLine("Debug: File read successfully, deserializing...");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        IgnoreNullValues = true,
                        WriteIndented = true
                    };
                    Company company = JsonSerializer.Deserialize<Company>(json, options) ?? new Company();

                  
                    foreach (var dept in company.departments)
                    {
                        dept.Employees.Clear();
                        foreach (var emp in company.employees.Where(e => e.DepartmentName == dept.Name))
                        {
                            dept.AddEmployee(emp);
                        }
                        var head = company.employees.FirstOrDefault(e => e.Id == dept.DepartmentHead.Id);
                        if (head != null) dept.DepartmentHead = head;
                    }

                    // Console.WriteLine("Debug: Data synced successfully.");
                    return company;
                }
                else
                {
                    // Console.WriteLine("Debug: File does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
            return new Company();
        }



        public void AddEmployee(Employee employee)
        {
            int nextId = employees.Any() ? employees.Max(e => e.Id) + 1 : 1;
            employee.Id = nextId;
            if (employees.Any(e => e.Id == employee.Id))
            {
                throw new Exception("Employee ID already exists.");
            }
            employees.Add(employee);
            var dept = departments.FirstOrDefault(d => d.Name == employee.DepartmentName);
            if (dept != null)
            {
                dept.AddEmployee(employee); // Add the same employee instance to department
            }
            else
            {
                throw new Exception($"Department {employee.DepartmentName} not found.");
            }
        }
       
        public void AddDepartment(Department department)
        {
            if (!departments.Any(d => d.Name == department.Name))
                departments.Add(department);
           
            
        } 
        public void PromoteEmployee(int id, JobTitle newTitle, decimal increasePercentage)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            if (employee.IsTerminated) throw new Exception("can't promote terminated employee !");

            
                employee.Promote(newTitle, increasePercentage);
                string message = $" {employee.Name} has been promoted to {newTitle} with a salary increase of {increasePercentage}%! New salary: {employee.Salary:C} 🚀";

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✓ {message}");
                Console.ResetColor();
           
        }

        





 
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
                Console.WriteLine(new string('-', 115));
                Console.WriteLine($"| {"ID",-10} | {"Name",-25} | {"Title",-25} | {"Salary",-20} | {"IsTerminated",-20} |");
                Console.WriteLine(new string('-', 115));

                // Employee details with alternating row colors
                bool alternateColor = false;

                foreach (var emp in department.GetEmployees())
                {
                    if (emp.IsTerminated)
                    {
                        Console.ForegroundColor = alternateColor ? ConsoleColor.Red : ConsoleColor.Red;
                        Console.WriteLine($"| {emp.Id,-10} | {emp.Name,-25} | {emp.Title,-25} | {emp.Salary.ToString("C"),-20} | {emp.IsTerminated,-20} ");
                        alternateColor = !alternateColor;
                    }
                    else
                    {
                        Console.ForegroundColor = alternateColor ? ConsoleColor.White : ConsoleColor.Gray;
                        Console.WriteLine($"| {emp.Id,-10} | {emp.Name,-25} | {emp.Title,-25} | {emp.Salary.ToString("C"),-20} | {emp.IsTerminated,-20} ");
                        alternateColor = !alternateColor;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(new string('-', 115));
                Console.WriteLine(); // Empty line between departments
                Console.ResetColor();
            }
        }


        public void GenerateTopPerformersReport()
        {
            var topPerformers = employees.Where(e => !e.IsTerminated && e.GetAveragePerformance() >= 4)
                                       .OrderByDescending(e => e.GetAveragePerformance());

            Console.WriteLine("\nTop Performers Report:");

            // Table header
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"| {"Employee Name",-25} | {"Average Rating",-30} |");
            Console.WriteLine(new string('-', 60));
            Console.ResetColor();

            // Employee details with alternating colors
            bool alternateColor = false;

            foreach (var emp in topPerformers)
            {
                Console.ForegroundColor = alternateColor ? ConsoleColor.White : ConsoleColor.Gray;
                Console.WriteLine($"| {emp.Name,-25} | {emp.GetAveragePerformance().ToString("F2"),-30} |");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 60));
                Console.ResetColor();
                alternateColor = !alternateColor;
            }
        }
       
        public void GenerateSalaryDistributionReport()
        {
            var activeEmployees = employees.Where(e => !e.IsTerminated);

            Console.WriteLine("\nSalary Distribution Report:");

            // Table header
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"| {"Metric",-30} | {"Value",-45} |");
            Console.WriteLine(new string('-', 80));
            Console.ResetColor();

            // Salary statistics with consistent formatting
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"| {"Average Salary",-30} | {activeEmployees.Average(e => e.Salary).ToString("C"),-45} |");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 80));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"| {"Highest Salary",-30} | {activeEmployees.Max(e => e.Salary).ToString("C"),-45} |");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 80));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"| {"Lowest Salary",-30} | {activeEmployees.Min(e => e.Salary).ToString("C"),-45} |");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 80));
            Console.ResetColor();
        }  
        public void TransferEmployeeToDepartment(string departmentName, int employeeId)
        {
            var employee = employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null) throw new InvalidOperationException("Employee not found.");

            // Debug: Print out employee details
            Console.WriteLine($"Attempting to transfer employee: {employee.Name}, ID: {employee.Id}, Department: {employee.DepartmentName}");

            // Find the old department by checking if the employee is in its list
            var oldDept = departments.FirstOrDefault(d => d.Employees.Contains(employee));

            if (oldDept == null)
            {
                Console.WriteLine("Old department not found. Employee might not be in any department's list.");
                // You might want to throw an exception or handle this case differently
            }
          
            var newDept = departments.FirstOrDefault(d => d.Name == departmentName);
            if (newDept == null) throw new InvalidOperationException("Target department not found.");

            employee.DepartmentName = departmentName;
            if (oldDept != null)
            {
                oldDept.RemoveEmployee(employee);
            }
            newDept.AddEmployee(employee);
        }

    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     