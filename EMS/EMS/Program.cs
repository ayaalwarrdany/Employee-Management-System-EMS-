using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EMS
{
    internal class Program
    {
        static List<Department> departments = new List<Department>();
        static List<Employee> employees = new List<Employee>();

        static void Main()
        {
            
            departments.Add(new Department("IT", null));
            departments.Add(new Department("HR", null));
            departments.Add(new Department("Sales", null));

            while (true)
            {
                Console.WriteLine("\n===== Employee Management System (EMS) =====");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. View Employees");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;
                    case "2":
                        ViewEmployees();
                        break;
                    case "3":
                        Console.WriteLine("Exiting... Thank you!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void AddEmployee()
        {
            string name;
            while (true)
            {
                Console.Write("Enter Employee Name (letters only): ");
                name = Console.ReadLine();

                if (!Regex.IsMatch(name, @"^[A-Za-z\s]+$"))
                {
                    Console.WriteLine("Invalid name. Please enter only letters.");
                    
                }
                else
                {
                    break;
                }
            }

            int age;
            while (true)
            {
                Console.Write("Enter Age (18-65): ");
                if (!int.TryParse(Console.ReadLine(), out age) || age < 18 || age > 65)
                {
                    Console.WriteLine("Invalid age. Please enter a number between 18 and 65.");
                }
                else
                {
                    break;
                }
            }

            double salary;
            while (true)
            {
                Console.Write("Enter Salary (must be positive): ");
                if (!double.TryParse(Console.ReadLine(), out salary) || salary <= 0)
                {
                    Console.WriteLine("Invalid salary. Please enter a positive number.");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Choose Department:");
            for (int i = 0; i < departments.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {departments[i].Name}");
            }

            int deptChoice;
            while (true)
            {
                Console.Write("Enter Department Number: ");
                if (!int.TryParse(Console.ReadLine(), out deptChoice) || deptChoice < 1 || deptChoice > departments.Count)
                {
                    Console.WriteLine("Invalid department selection. Try again.");
                }
                else
                {
                    break;
                }
            }

            Department selectedDepartment = departments[deptChoice - 1];
            Employee newEmployee = new Employee(name, age, salary, selectedDepartment);
            employees.Add(newEmployee);

            Console.WriteLine($"Employee {name} added successfully.");
        }

        static void ViewEmployees()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees available.");
                return;
            }

            Console.WriteLine("\n===== Employee List =====");
            foreach (var emp in employees)
            {
                Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Age: {emp.Age}, Salary: {emp.Salary:C}, " +
                                  $"Department: {emp.Department.Name}, Hired On: {emp.EmployeeDate:yyyy-MM-dd}, " +
                                  $"Terminated: {emp.IsTerminated}");
            }
        }
    }
}
