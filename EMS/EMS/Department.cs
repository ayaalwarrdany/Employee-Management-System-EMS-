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
            if(employee == null) throw new Exception("Employee is not Found");
            Employees.Add(employee);
            Console.WriteLine($"{employee.Name} is Added to {Name} Department");
        }


        public void RemoveEmployee(Employee employee)
        {
            Employees.Remove(employee);
            Console.WriteLine($"{employee.Name} is removed from {Name} Department");
        }
        public List<Employee> GetEmployees()
        {
            return Employees;
        }
    }
}
