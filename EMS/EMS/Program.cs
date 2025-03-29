﻿using Employee_Management_System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EMS
{

    internal class Program
    {
        static Company company = new Company();

        static void Main(string[] args)
        {
            string filePath = "companyData.json";
            company = Company.LoadFromFile(filePath);
            //InitializeSampleData();

            bool running = true;

            Console.Title = "Employee Management System";
            while (running)
            {
                Console.Clear();
                DisplayMenu();
                Console.Write(">> ");
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddDepartment(); break;
                        case "2": AddEmployee(); break;
                        case "3": AddPerformanceReview(); break;
                        case "4": PromoteEmployee(); break;
                        case "5": TransferEmployee(); break;
                        case "6": TerminateEmployee(); break;
                      
                        case "7": GenerateReports(); break;
                        case "8":
                          
                            DisplayFooter();
                            company.SaveToFile(filePath);

                            break;
                        case "0":
                            Console.WriteLine("\nExiting... Press any key to close.");
                            Console.ReadKey();
                            running = false; break;
                        default: DisplayError("Invalid option."); break;
                    }
                }
                catch (Exception ex)
                {
                    DisplayError($"Error: {ex.Message}");
                }
                if (choice != "0") WaitForUser();
            }
        }

        static void DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║   EMPLOYEE MANAGEMENT SYSTEM       ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\nPlease select an option:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  [1] Add Department");
            Console.WriteLine("  [2] Add Employee");
            Console.WriteLine("  [3] Add Performance Employee");
            Console.WriteLine("  [4] Promote Employee");
            Console.WriteLine("  [5] Transfer Employee");
            Console.WriteLine("  [6] Terminate Employee");
            Console.WriteLine("  [7] Generate Reports");
            Console.WriteLine("  [8] Save");
            Console.WriteLine("  [0] Exit");

            Console.ResetColor();
        }

        static void AddEmployee()
        {
            Console.Clear();
            DisplayHeader("Add Employee");

            string name = GetValidName("Enter Employee Name (letters only): ");
            int age = GetValidInt("Enter Age (18-65): ", 18, 65);
            decimal salary = GetValidDecimal("Enter Salary (must be positive): ", 0, decimal.MaxValue);

            // Check if departments are loaded from the file
            if (company.departments.Count == 0)
            {
                DisplayWarning("No departments available. Please add a department first or ensure companyData.json contains department data.");
                return;
            }

            // Display available departments loaded from the file
            Department selectedDept = SelectDepartment();
            if (selectedDept == null) return;

            Employee newEmployee = new Employee(name, age, (decimal)salary, selectedDept.Name);
            company.AddEmployee(newEmployee);
            DisplaySuccess($"Employee {name} added successfully to {selectedDept.Name}.");
        }
        static void AddDepartment()
        {
            Console.Clear();
            DisplayHeader("Add Department");

            string deptName = GetValidName("Enter Department Name (letters only): ");

            if (!company.departments.Any(d => d.Name == deptName))
            {
                DisplaySubHeader("Department Head Information");
                string headName = GetValidName("Enter Head's Name (letters only): ");
                int headAge = GetValidInt("Enter Head's Age (18-65): ", 18, 65);
                decimal headSalary = GetValidDecimal("Enter Head's Salary (must be positive): ", 0, decimal.MaxValue);



                Employee newHead = new Employee(headName, headAge, (decimal)headSalary, deptName);


                company.AddDepartment(new Department(deptName, newHead));

                company.AddEmployee(newHead);


                DisplaySuccess($"Department {deptName} added successfully with {headName} as head.");

            }

            else
            {
                DisplayWarning($"The Department Name  {deptName} is Already Added ");
                

            }

        }

        //static void PromoteEmployee()
        //{
        //    Console.Clear();
        //    DisplayHeader("Promote Employee");
        //    company.GenerateDepartmentReport();
        //    Employee emp = SelectEmployee("Enter Employee ID to promote: ");
        //    if (emp == null) return;

        //    string newTitle = GetValidName("Enter New Title: ");
        //    decimal increase = GetValidDecimal("Enter Salary Increase (must be positive): ", 0, decimal.MaxValue);

        //    // Company.PromoteEmployee(emp.Id, newTitle, increase);
        //    DisplaySuccess($"Employee {emp.Name} promoted successfully.");
        //}

        static void PromoteEmployee()
        {
            Console.Clear();
            DisplayHeader("Promote Employee");
            company.GenerateDepartmentReport();
            Employee emp = SelectEmployee("Enter Employee ID to promote: ");
            if (emp == null) return;

            JobTitle newTitle = SelectJobTitle();
            decimal increase = GetValidDecimal("Enter Salary Increase Percentage: ", 0, decimal.MaxValue);

            try
            {
                company.PromoteEmployee(emp.Id, newTitle, increase);
            }
            catch (Exception ex)
            {
                DisplayError($"Promotion failed: {ex.Message}");
            }
            WaitForUser();
        }
        //static void PromoteEmployee()
        //{
        //    Console.Clear();
        //    DisplayHeader("Promote Employee");
        //    company.GenerateDepartmentReport();

        //    Employee emp = SelectEmployee("Enter Employee ID to promote: ");
        //    if (emp == null) return;

        //    JobTitle newTitle = SelectJobTitle();
        //    decimal increasePercentage = GetValidDecimal("Enter Salary Increase Percentage: ", 0, decimal.MaxValue);

        //    try
        //    {
        //        company.PromoteEmployee(emp.Id, newTitle, (decimal)increasePercentage);
        //        DisplaySuccess($"Employee {emp.Name} promoted to {newTitle} with a salary increase of {increasePercentage}%.");
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayError($"Promotion failed: {ex.Message}");
        //    }
        //}


        static void TransferEmployee()
        {

            Console.Clear();
            DisplayHeader("Transfer Employee");
            company.GenerateDepartmentReport();

            Employee emp = SelectEmployee("Enter Employee ID to transfer: ");
            if (emp == null) return;
            if (emp.IsTerminated)
            {
                DisplayWarning($"Employee {emp.Name} is terminated and cannot be transferred.");
                return;
            }
            Department oldDepartment = company.departments.FirstOrDefault(dept => dept.Name == emp.DepartmentName);
            if(emp.Id == oldDepartment.DepartmentHead.Id)
            {
                DisplayWarning($"Employee {emp.Name} is The Department Head  and cannot be transferred.");

                return;
            }





                Department newDept = SelectDepartment();
            if (newDept == null) return;
            if (newDept.Name == emp.DepartmentName)
            {
                DisplayError($"Employee {emp.Name} is already in {newDept.Name}.");
            }
          
           

            
            else
            {
                company.TransferEmployeeToDepartment(newDept.Name, emp.Id);
                DisplaySuccess($"Employee {emp.Name} transferred successfully.");
            }
        }

        static void TerminateEmployee()

        {

            Console.Clear();

            DisplayHeader("Terminate Employee");

            company.GenerateDepartmentReport();

            Employee emp = SelectEmployee("Enter Employee ID to terminate: ");

            if (emp == null) return;

            if (emp.IsTerminated)

            {

                DisplaySuccess($"Employee {emp.Name} has already been Terminated!");

            }

            else

            {

                emp.Terminate();

                DisplaySuccess($"Employee {emp.Name} terminated successfully.");

            }

        }


        static void AddPerformanceReview()
        {
            Console.Clear();
            DisplayHeader("Add Performance Review");
            company.GenerateDepartmentReport();

            Employee emp = SelectEmployee("Enter Employee ID for review: ");
            if (emp == null) return;

            DisplaySubHeader("Performance Rating");
            Console.WriteLine("  [1] Poor");
            Console.WriteLine("  [2] Fair");
            Console.WriteLine("  [3] Good");
            Console.WriteLine("  [4] Very Good");
            Console.WriteLine("  [5] Excellent");
            int ratingChoice = GetValidInt("Select Rating Number: ", 1, 5);
            PerformanceRating rating = (PerformanceRating)ratingChoice;

            Console.Write("Enter Comments (optional): ");
            string comments = Console.ReadLine();

            emp.AddPerformanceReview(rating, comments);
            WaitForUser();
        }
        //static void AddPerformanceReview()
        //{
        //    Console.Clear();
        //    DisplayHeader("Add Performance Review");

        //    company.GenerateDepartmentReport();

        //    Employee emp = SelectEmployee("Enter Employee ID for review: ");
        //    if (emp == null) return;

        //    double rating = GetValidDouble("Enter Rating (1-5): ", 1, 5);
        //    // emp.AddPerformanceReview(new PerformanceReview(DateTime.Now, rating));
        //    DisplaySuccess("Performance review added successfully.");
        //}

        static void GenerateReports()
        {
            Console.Clear();
            DisplayHeader("Generate Reports");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Select Report Type:");
            Console.WriteLine("  [1] Employees per Department");
            Console.WriteLine("  [2] Top Performers");
            Console.WriteLine("  [3] Salary Distribution");
            Console.ResetColor();
            Console.Write(">> ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1": company.GenerateDepartmentReport(); break;
                case "2": company.GenerateTopPerformersReport(); break;
                case "3": company.GenerateSalaryDistributionReport(); break;
                default: DisplayError("Invalid report option."); break;
            }
        }

        // UI Helper Methods
        static void DisplayHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"╔══════ {title.ToUpper()} ══════╗");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void DisplaySubHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n--- {title} ---");
            Console.ResetColor();
        }

        static void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {message}");
            Console.ResetColor();
        }

        static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {message}");
            Console.ResetColor();
        }

        static void DisplayWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n! {message}");
            Console.ResetColor();
        }

        static void DisplayFooter()
        {
            Console.WriteLine("\n╔════════════════════════════════════╗");
            Console.WriteLine($"║ Employees: {company.employees.Count,-5} Departments: {company.departments.Count,-5} ║");
            Console.WriteLine("╚════════════════════════════════════╝");
        }

        static void WaitForUser()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static string GetValidName(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^[A-Za-z\s]+$"))
                    return input;
                DisplayError("Invalid input. Please use letters only.");
                Console.Write(prompt);
            }
        }

        static int GetValidInt(string prompt, int min, int max)
        {
            Console.Write(prompt);
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;
                DisplayError($"Invalid input. Please enter a number between {min} and {max}.");
                Console.Write(prompt);
            }
        }

        static decimal GetValidDecimal(string prompt, decimal min, decimal max)
        {
            Console.Write(prompt);
            while (true)
            {
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value > min && value <= max)
                    return value;
                DisplayError($"Invalid input. Please enter a number greater than {min}.");
                Console.Write(prompt);
            }
        }

        static double GetValidDouble(string prompt, double min, double max)
        {
            Console.Write(prompt);
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out double value) && value >= min && value <= max)
                    return value;
                DisplayError($"Invalid input. Please enter a number between {min} and {max}.");
                Console.Write(prompt);
            }
        }

        static Department SelectDepartment()
        {
            if (company.departments.Count == 0) return null;

            DisplaySubHeader("Available Departments");
            for (int i = 0; i < company.departments.Count; i++)
                Console.WriteLine($"  [{i + 1}] {company.departments[i].Name}");

            int choice = GetValidInt("Select Department Number: ", 1, company.departments.Count);
            return company.departments[choice - 1];
        }

        static Employee SelectEmployee(string prompt)
        {
            if (company.employees.Count == 0)
            {
                DisplayWarning("No employees available.");
                return null;
            }

            int id = GetValidInt(prompt, 1, int.MaxValue);
            Employee emp = company.employees.FirstOrDefault(e => e.Id == id);
            if (emp == null)
                DisplayError("Employee not found.");
            return emp;
        }

        //static void InitializeSampleData()
        //{
        //    if (Company.employees.Count == 0 && Company.departments.Count == 0)
        //    {
        //        var emp1 = new Employee("Alice Smith", 30, 60000, "HR");
        //        var emp2 = new Employee("Bob Jones", 45, 80000, "IT");
        //        company.AddEmployee(emp1);
        //        company.AddEmployee(emp2);
        //        company.AddDepartment(new Department("IT", emp1));
        //        company.AddDepartment(new Department("HR", emp2));
        //    }
        //}

        static JobTitle SelectJobTitle()
        {
            // Convert enum to list of strings
            var jobTitles = Enum.GetValues(typeof(JobTitle)).Cast<JobTitle>().ToList();

            // Display options
            DisplaySubHeader("Available Job Titles");
            for (int i = 0; i < jobTitles.Count; i++)
            {
                Console.WriteLine($"  [{i + 1}] {jobTitles[i]}");
            }

            // Get user choice
            int choice = GetValidInt("Select Job Title Number: ", 1, jobTitles.Count);

            // Convert choice to enum
            return jobTitles[choice - 1];
        }


    }
}


/*
 * 
 
  Console.Clear();
     DisplayHeader("Transfer Employee");
     company.GenerateDepartmentReport();
 
     Employee emp = SelectEmployee("Enter Employee ID to transfer: ");
     if (emp == null) return;
     if (emp.IsTerminated)
     {
         DisplayWarning($"Employee {emp.Name} is terminated and cannot be transferred.");
         return;
     }
 
     Department newDept = SelectDepartment();
     if (newDept == null) return;
     if (newDept.Name == emp.DepartmentName)
     {
         DisplayError($"Employee {emp.Name} is already in {newDept.Name}.");
     }
     else
     {
         company.TransferEmployeeToDepartment(newDept.Name, emp.Id);
         DisplaySuccess($"Employee {emp.Name} transferred successfully.");
     }
}






 * 
 * 
 
using Employee_Management_System;
using EMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace EMS
{
    internal class Program
    {
       // static List<Department> departments = new List<Department>();
         
        
            static void Main(string[] args)
            {
                string filePath = "companyData.json";
                Company company = Company.LoadFromFile(filePath); // Load existing data
                InitializeSampleData(); // Add sample data if needed
                bool running = true;

                while (running)
                {
                    Console.WriteLine("\nEmployee Management System");
                    Console.WriteLine("1. Add Employee");
                    Console.WriteLine("2. Add Department");
                    Console.WriteLine("3. Promote Employee");
                    Console.WriteLine("4. Transfer Employee");
                    Console.WriteLine("5. Terminate Employee");
                    Console.WriteLine("6. Add Performance Review");
                    Console.WriteLine("7. Generate Reports");
                    Console.WriteLine("8. Exit");
                    Console.Write("Select an option: ");

                    string choice = Console.ReadLine();

                    try
                    {
                        switch (choice)
                        {
                            case "1":
                            AddEmployee();
                            break;

                            case "2":
                           
                            //    Console.Write("Dept Name: "); string deptName = Console.ReadLine();
                            //    Console.Write("Head ID: "); int headId = int.Parse(Console.ReadLine());
                            //    var head = company.employees.FirstOrDefault(e => e.ID == headId);
                            //    if (head != null) company.AddDepartment(new Department(deptName, head));
                            //    else Console.WriteLine("Head not found.");
                            //    break;

                            //case "3":
                            //    Console.Write("Employee ID: "); int promId = int.Parse(Console.ReadLine());
                            //    Console.Write("New Title: "); string newTitle = Console.ReadLine();
                            //    Console.Write("Salary Increase: "); decimal increase = decimal.Parse(Console.ReadLine());
                            //    company.PromoteEmployee(promId, newTitle, increase);
                            //    break;

                            //case "4":
                            //    Console.Write("Employee ID: "); int transId = int.Parse(Console.ReadLine());
                            //    Console.Write("New Department: "); string newDept = Console.ReadLine();
                            //    company.TransferEmployee(transId, newDept);
                            //    break;

                            //case "5":
                            //    Console.Write("Employee ID: "); int termId = int.Parse(Console.ReadLine());
                            //    var empToTerm = company.employees.FirstOrDefault(e => e.ID == termId);
                            //    if (empToTerm != null) empToTerm.Terminate();
                            //    break;

                            //case "6":
                            //    Console.Write("Employee ID: "); int revId = int.Parse(Console.ReadLine());
                            //    Console.Write("Rating (1-5): "); double rating = double.Parse(Console.ReadLine());
                            //    var emp = Company.employees.FirstOrDefault(e => e.ID == revId);
                            //    if (emp != null) emp.AddPerformanceReview(new PerformanceReview(DateTime.Now, rating));
                            //    break;

                            //case "7":
                            //    Console.WriteLine("\n1. Employees per Department\n2. Top Performers\n3. Salary Distribution");
                            //    string reportChoice = Console.ReadLine();
                            //    if (reportChoice == "1") company.GenerateDepartmentReport();
                            //    else if (reportChoice == "2") company.GenerateTopPerformersReport();
                            //    else if (reportChoice == "3") company.GenerateSalaryDistributionReport();
                            //    break;

                            //case "8":
                            //    running = false;
                            //    Console.WriteLine($"Employees: {company.employees.Count}, Departments: {company.departments.Count}"); // Updated to Employees and Departments
                            //    company.SaveToFile(filePath);
                            //    Console.WriteLine("Exiting...");
                            //    break;

                            default:
                                Console.WriteLine("Invalid option.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }











            static void InitializeSampleData()
        {
            // Department department = new Department("IT");
            Company company = new Company(); 
            if (Company.employees.Count == 0 && Company.departments.Count == 0)
            {

                var emp1 = new Employee("Alice Smith", 30, 60000, "HR");
                var emp2 = new Employee("Bob Jones", 45, 80000, "IT");
                var emp3 = new Employee("Alice Smith", 30, 60000, "HR");
                company.AddEmployee(emp1);
                company.AddEmployee(emp2);
                company.AddEmployee(emp3);
                company.AddDepartment(new Department("IT", emp1));
                company.AddDepartment(new Department("HR", emp2));
                company.AddDepartment(new Department("Sales", emp1));
 
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

           // Company company = new Company();
            Console.WriteLine("Choose Department:");
            for (int i = 0; i < Company.departments.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Company.departments[i].Name}");
            }

            int Choice;
            while (true)
            {
                Console.Write("Enter Department Number: ");
                if (!int.TryParse(Console.ReadLine(), out Choice) || Choice < 1 || Choice > Company.departments.Count)
                {
                    Console.WriteLine("Invalid department selection. Try again.");
                }
                else
                {
                    break;
                }
            }

            Department selectedDepartment = Company.departments[Choice - 1];
            Employee newEmployee = new Employee(name, age, salary, selectedDepartment.Name);
            Company.employees.Add(newEmployee);
            //employees.Add(newEmployee);

            Console.WriteLine($"Employee {name} added successfully.");
        }


        //ViewEmployee
        //static void ViewEmployees()
        //{
        //    if (employees.Count == 0)
        //    {
        //        Console.WriteLine("No employees available.");
        //        return;
        //    }

        //    Console.WriteLine("\n===== Employee List =====");
        //    foreach (var emp in employees)
        //    {
        //        Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Age: {emp.Age}, Salary: {emp.Salary:C}, " +
        //                          $"Department: {emp.Department.Name}, Hired On: {emp.EmployeeDate:yyyy-MM-dd}, " +
        //                          $"Terminated: {emp.IsTerminated}");
        //    }
        //}







        ////TransferEmployee
        //static void TransferEmployee()
        //{
        //    Console.Write("Enter Employee ID to Transfer: ");
        //    if (!int.TryParse(Console.ReadLine(), out int id))
        //    {
        //        Console.WriteLine("Invalid ID.");
        //        return;
        //    }

        //    Employee emp = employees.FirstOrDefault(e => e.Id == id);
        //    if (emp == null)
        //    {
        //        Console.WriteLine("Employee not found.");
        //        return;
        //    }
        //    if (emp.IsTerminated)
        //    {
        //        Console.WriteLine($"Employee {emp.Name} is terminated and cannot be transferred.");
        //        return;
        //    }

        //    Console.WriteLine("Choose New Department:");
        //    for (int i = 0; i < departments.Count; i++)
        //        Console.WriteLine($"{i + 1}. {departments[i].Name}");

        //    Console.Write("Enter Department Number: ");
        //    if (!int.TryParse(Console.ReadLine(), out int deptChoice) || deptChoice < 1 || deptChoice > departments.Count)
        //    {
        //        Console.WriteLine("Invalid department selection.");
        //        return;
        //    }
        //    Department newDepartment = departments[deptChoice - 1];
        //    if (emp.Department == newDepartment)
        //    {
        //        Console.WriteLine($"Employee {emp.Name} is already in {newDepartment.Name}.");
        //        return;
        //    }
        //    emp.TransferDepartment(newDepartment);
        //    Console.WriteLine($"Employee {emp.Name} has been transferred to {newDepartment.Name}.");

        //}




        ////TerminateEmployee
        //static void TerminateEmployee()
        //{
        //    Console.Write("Enter Employee ID to Terminate: ");
        //    if (!int.TryParse(Console.ReadLine(), out int id))
        //    {
        //        Console.WriteLine("Invalid ID.");
        //        return;
        //    }
        //    Employee emp = employees.FirstOrDefault(e => e.Id == id);
        //    if (emp == null)
        //    {
        //        Console.WriteLine("Employee not found.");
        //        return;
        //    }
        //    emp.Terminate();
        //}

                #endregion
            
      
    }
} 


 */