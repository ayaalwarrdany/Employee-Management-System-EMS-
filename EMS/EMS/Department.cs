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
        public List<Employee> Employees { get; set; }

        public Department(string name, Employee departmentHead)
        {
            Name = name;
            Employees = new List<Employee>();
            DepartmentHead = departmentHead;
           
        }
       
        public void AddEmployee(Employee employee)
        {
            if (employee == null) throw new Exception("Employee is not Found");
            Employees.Add(employee);
              }

 
        public void RemoveEmployee(Employee employee)
        {

            if (employee == null) throw new ArgumentNullException(nameof(employee), "Employee cannot be null");

            Employees.Remove(employee);

           
        }
        public List<Employee> GetEmployees()
        {
            return Employees;
        }
    }
}
