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
        private List<Employee> employees;
        public Department(string name)
        {
                Name = name;
            employees = new List<Employee>();
            
        }
        public void AddEmployee(Employee employee) 
        {
        employees.Add(employee);
        }    
        public void RemoveEmployee(Employee employee) 
        {
        employees.Remove(employee);
        }
    }
}
