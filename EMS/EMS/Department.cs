using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS
{
    internal class Department
    {
        //d : must emp a head /  demaptName 
        public string Name { get; set; }
        public Employee DepartmentHead { get; set; }
        public List<Employee> Employees { get; set; }

        public Department(string name, Employee departmentHead)
        {
            Name = name;
            // DepartmentHead = departmentHead;
            Employees = new List<Employee>();
            DepartmentHead = departmentHead;
            //  Employees.Add(departmentHead);
            //if (departmentHead != null)
            //{
            //    Employees.Add(departmentHead);
            //}
        }
        // fun : addDepHead (emp){}
        public void AddEmployee(Employee employee)
        {
            if (employee == null) throw new Exception("Employee is not Found");
            Employees.Add(employee);
            //  Console.WriteLine($"{employee.Name} is Added to {Name} Department");
        }


        // public void RemoveEmployee(Employee employee)
        //{

        //   bool isRemoved = Employees.Remove(employee);

        //    Console.WriteLine($"removed : {isRemoved}");
        //    Console.WriteLine($"{employee.Name} is removed from {Name} Department");
        //}
        public void RemoveEmployee(Employee employee)
        {

            if (employee == null) throw new ArgumentNullException(nameof(employee), "Employee cannot be null");

            /*bool isRemoved =*/
            Employees.Remove(employee);

            //if (isRemoved)
            //{
            //   // Console.WriteLine($"{employee.Name} is successfully removed from {Name} Department");
            //}
            //else
            //{
            //    //Console.WriteLine($"Failed to remove {employee.Name} from {Name} Department. Employee was not found in the department.");
            //    //Console.WriteLine($"Department Employees Count: {Employees.Count}");
            //    //foreach (var emp in Employees)
            //    //{
            //    //    Console.WriteLine($"Employee in department: {emp.Name}, ID: {emp.Id}");
            //    //}
            //    //Console.WriteLine($"Employee to remove: {employee.Name}, ID: {employee.Id}");

            //}
        }
        public List<Employee> GetEmployees()
        {
            return Employees;
        }
    }
}
