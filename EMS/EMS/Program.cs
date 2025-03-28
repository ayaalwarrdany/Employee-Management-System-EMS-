using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
                Console.WriteLine("3. TransferEmployee");
                Console.WriteLine("4. TerminateEmployee");
                Console.WriteLine("5. Exit");
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
                        TransferEmployee();
                        break;
                    case "4":
                        TerminateEmployee();
                        break;
                    case "5":
                        Console.WriteLine("Exiting... Thank you!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            #region Employee
            //AddEmployee
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

                int Choice;
                while (true)
                {
                    Console.Write("Enter Department Number: ");
                    if (!int.TryParse(Console.ReadLine(), out Choice) || Choice < 1 || Choice > departments.Count)
                    {
                        Console.WriteLine("Invalid department selection. Try again.");
                    }
                    else
                    {
                        break;
                    }
                }

                Department selectedDepartment = departments[Choice - 1];
                Employee newEmployee = new Employee(name, age, salary, selectedDepartment);
                employees.Add(newEmployee);

                Console.WriteLine($"Employee {name} added successfully.");
            }

            //ViewEmployee
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

            //TransferEmployee
            static void TransferEmployee()
            {
                Console.Write("Enter Employee ID to Transfer: ");
                if (!int.TryParse(Console.ReadLine(), out int id)) 
                {
                    Console.WriteLine("Invalid ID."); 
                    return;
                }
          
                Employee emp = employees.FirstOrDefault(e => e.Id == id);
                if (emp == null) 
                {
                    Console.WriteLine("Employee not found.");
                    return;
                }
                if (emp.IsTerminated)
                {
                    Console.WriteLine($"Employee {emp.Name} is terminated and cannot be transferred.");
                    return;
                }

                Console.WriteLine("Choose New Department:");
                for (int i = 0; i < departments.Count; i++)
                    Console.WriteLine($"{i + 1}. {departments[i].Name}");

                Console.Write("Enter Department Number: ");
                if (!int.TryParse(Console.ReadLine(), out int deptChoice) || deptChoice < 1 || deptChoice > departments.Count)
                {
                    Console.WriteLine("Invalid department selection.");
                    return;
                }
                Department newDepartment = departments[deptChoice - 1];
                if(emp.Department == newDepartment)
                {
                    Console.WriteLine($"Employee {emp.Name} is already in {newDepartment.Name}.");
                    return;
                }
                emp.TransferDepartment(newDepartment);
                Console.WriteLine($"Employee {emp.Name} has been transferred to {newDepartment.Name}.");

            }

            //TerminateEmployee
            static void TerminateEmployee()
            {
                Console.Write("Enter Employee ID to Terminate: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID."); 
                    return;
                }
                Employee emp = employees.FirstOrDefault(e => e.Id == id);
                if (emp == null) 
                { 
                    Console.WriteLine("Employee not found."); 
                    return;
                }
                emp.Terminate();
            }

            #endregion
        }
    }
}
