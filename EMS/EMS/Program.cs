using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
            Console.WriteLine("  [3] Add Performance Review");
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
            if (company.departments.Count == 0)
            {
                DisplayWarning("No departments available. Please add a department first or ensure companyData.json contains department data.");
                return;
            }
            string name = GetValidName("Enter Employee Name (letters only): ");
            int age = GetValidInt("Enter Age (18-65): ", 18, 65);

            JobTitle newTitle = SelectJobTitle();

            decimal salary = GetValidDecimal("Enter Salary (must be positive): ", 0, decimal.MaxValue);

             
            Department selectedDept = SelectDepartment();
            if (selectedDept == null) return;

            Employee newEmployee = new Employee(name, age,newTitle, (decimal)salary, selectedDept.Name);
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

                JobTitle newTitle = SelectJobTitle();

                decimal headSalary = GetValidDecimal("Enter Head's Salary (must be positive): ", 0, decimal.MaxValue);



                Employee newHead = new Employee(headName, headAge,newTitle, (decimal)headSalary, deptName);


                company.AddDepartment(new Department(deptName, newHead));

                company.AddEmployee(newHead);


                DisplaySuccess($"Department {deptName} added successfully with {headName} as head.");

            }

            else
            {
                DisplayWarning($"The Department Name  {deptName} is Already Added ");
                //   // do u wanna edit ? 

            }

        }

     
        static void PromoteEmployee()
        {
            Console.Clear();
            DisplayHeader("Promote Employee");
            company.GenerateDepartmentReport();
            Employee emp = SelectEmployee("Enter Employee ID to promote: ");
            if (emp == null) return;
            if (emp.IsTerminated)
            {
               string message = "can't promote a terminated employee !";
                DisplayWarning(message);
                return;
            }
            if (!emp.IsEligibleForPromotion()) {

                 
                DisplayWarning($"{emp.Name} is not eligible for promotion.");
                return;
            }


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
           
        }
       


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
                // edit  


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

                DisplayWarning($"Employee {emp.Name} has already been Terminated!");
  

                return;

            }
            Department oldDepartment = company.departments.FirstOrDefault(dept => dept.Name == emp.DepartmentName);
            if (emp.Id == oldDepartment.DepartmentHead.Id)
            {
                DisplayWarning($"Employee {emp.Name} is The Department Head  and cannot be Terminated.");

                // do u wanna edit // add head ? 





                return;
            }

            else

            {

                emp.Terminate();

                DisplaySuccess($"Employee {emp.Name} terminated successfully.");

            }

        }


        static void AddPerformanceReview()
        {
         
            int currentMonth = DateTime.Now.Month;
            if (currentMonth != 12 && currentMonth !=3 && currentMonth != 6 && currentMonth != 9)
            {
                string message = "❌ This is not a review quarter (Des, Mar, Jun, Sep). Performance review cannot be added.";
                DisplayWarning(message);
                return;
            }  
            Console.Clear();
            DisplayHeader("Add Performance Review");
            company.GenerateDepartmentReport();

            Employee emp = SelectEmployee("Enter Employee ID for review: ");
            if (emp == null) return;
            if (emp.IsTerminated)
            {
                string message = "can't promote a terminated employee !";
                DisplayError(message);
                return;
            }
            PerformanceReview oldPer = emp.performanceReviews.FirstOrDefault(p => p.ReviewDate.Month == currentMonth);
            if (oldPer != null)
            {
                DisplayWarning("The Perofrmance Review already Added for this Quarter ");
                DisplayHeader("Do You Want To Edit The Performance Review For This Quarter ?");

                
                Console.WriteLine("  [1] Yes");
                Console.WriteLine("  [2] No");
                int editChoise = GetValidInt(" Select an Option : ", 1, 2);
                if(editChoise == 2)
                {
                    return;
                }
            }


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
          //  WaitForUser();
        }
        

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
 