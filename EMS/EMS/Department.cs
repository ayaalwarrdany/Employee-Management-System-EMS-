using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS
{
    internal class Department
    {
        public string Name { get; set; }
        public Employee DepartmentHead { get; set; }
        public List<Employee> Employees { get; private set; }

        public Department(string name, Employee departmentHead)
        {
            Name = name;
            DepartmentHead = departmentHead;
            Employees = new List<Employee>();
            Employees.Add(departmentHead);
            //if (departmentHead != null)
            //{
            //    Employees.Add(departmentHead);
            //}
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null) throw new Exception("Employee is not Found");
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
