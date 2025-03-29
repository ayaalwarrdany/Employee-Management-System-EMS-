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

namespace Employee_Management_System
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
                dept.AddEmployee(employee); // Ensure this line is executed
            }
            else
            {
                throw new Exception($"Department {employee.DepartmentName} not found.");
            }
        }
        //public void AddEmployee(Employee employee)
        //{
        //    int nextId = employees.Any() ? employees.Max(e => e.Id) + 1 : 1;
        //    employee.Id = nextId;
        //    if (employees.Any(e => e.Id == employee.Id))
        //    {

        //        throw new Exception("Employee ID already exists.");
        //    }
        //    employees.Add(employee);
        //    var dept = departments.FirstOrDefault(d => d.Name == employee.DepartmentName);
        //    dept?.AddEmployee(employee);
        //}

        public void AddDepartment(Department department)
        {
            if (!departments.Any(d => d.Name == department.Name))
                departments.Add(department);
            else
            {

            }
            
        }
        //   Report Generation

        public void PromoteEmployee(int id, JobTitle newTitle, decimal increasePercentage)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            if (employee.IsEligibleForPromotion())
            {
                employee.Promote(newTitle, increasePercentage);
                Console.WriteLine($"🎉 {employee.Name} has been promoted to {newTitle} with a salary increase of {increasePercentage}%! New salary: {employee.Salary:C} 🚀");
            }
            else
            {
                Console.WriteLine($"🚫 {employee.Name} is not eligible for promotion.");
            }
        }

        //public void PromoteEmployee(int id, JobTitle newTitle, decimal increasePercentage)
        //{
        //    var employee = employees.FirstOrDefault(e => e.Id == id);
        //    if (employee != null) //&& employee.GetAveragePerformance() >= 4.0)
        //    {
        //        employee.Promote(newTitle, increasePercentage);
        //    }
        //    //else
        //    //    throw new Exception("Employee not found or not eligible for promotion(rating < 4.0).");

        //}







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
            var topPerformers = employees.Where(e => !e.IsTerminated && e.GetAveragePerformance() >= 4.5)
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
        //public void GenerateTopPerformersReport()
        //{
        //    var topPerformers = employees.Where(e => !e.IsTerminated && e.GetAveragePerformance() >= 4.5).OrderByDescending(e => e.GetAveragePerformance());
        //    Console.WriteLine("\nTop Performers:");
        //    foreach (var emp in topPerformers)
        //        Console.WriteLine($"{emp.Name} - Avg Rating: {emp.GetAveragePerformance()}");
        //}


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
        //public void GenerateSalaryDistributionReport()
        //{
        //    Console.WriteLine("\nSalary Distribution:");
        //    var activeEmployees = employees.Where(e => !e.IsTerminated);
        //    Console.WriteLine($"Average Salary: {activeEmployees.Average(e => e.Salary)}");
        //    Console.WriteLine($"Highest Salary: {activeEmployees.Max(e => e.Salary)}");
        //    Console.WriteLine($"Lowest Salary: {activeEmployees.Min(e => e.Salary)}");
        //}




        //public void TransferEmployeeToDepartment(string departmentName , int employeeId)
        //{

        //    var employee = employees.FirstOrDefault(e => e.Id ==employeeId);
        //    if (employee == null) throw new InvalidOperationException("Employee not found.");
        //     var oldDept = departments.FirstOrDefault(d => d.Name == employee.DepartmentName);
        //   // var oldDept = departments.FirstOrDefault(d => d.Employees.Contains(employee));

        //    var newDept = departments.FirstOrDefault(d => d.Name == departmentName);
        //    if (newDept == null) throw new InvalidOperationException("Target department not found.");


        //      employee.DepartmentName = departmentName;
        //    if (oldDept != null)
        //    {
        //        Console.WriteLine("wooooow");
        //        oldDept?.RemoveEmployee(employee);
        //    }
        //    else { Console.WriteLine("nooooooooooooon"); }
        //        newDept.AddEmployee(employee);

        //    //Department oldDepartment= 
        //    //Department.RemoveEmployee(this);
        //    //newDepartment.AddEmployee(this);
        //    //Department = newDepartment;

        //}
        public void TransferEmployeeToDepartment(string departmentName, int employeeId)
        {
            var employee = employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null) throw new InvalidOperationException("Employee not found.");

            // Debug: Print out employee details
            Console.WriteLine($"Attempting to transfer employee: {employee.Name}, ID: {employee.Id}, Department: {employee.DepartmentName}");

            // Find the old department by checking if the employee is in its list
            var oldDept = departments.FirstOrDefault(d => d.Employees.Contains(employee));

            // Debug: Check all departments and their employees
            //Console.WriteLine("Checking all departments for the employee:");
            //foreach (var dept in departments)
            //{
            //    Console.WriteLine($"Department: {dept.Name}");
            //    foreach (var emp in dept.Employees)
            //    {
            //        Console.WriteLine($"  Employee: {emp.Name}, ID: {emp.Id}");
            //    }
            //}

            if (oldDept == null)
            {
                Console.WriteLine("Old department not found. Employee might not be in any department's list.");
                // You might want to throw an exception or handle this case differently
            }
            //else
            //{
            //    Console.WriteLine($"Old department found: {oldDept.Name}");
            //}





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
}  /* 
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
 
             */
