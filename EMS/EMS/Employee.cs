using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EMS
{
    internal class Employee
    {
        public int Id { get;private set; }
        private static int nextId = 1;
        public string Name { get;private set; }
        public int Age { get; private set; }
        public decimal Salary { get; private set; }
        public Department Department { get; private set; }
        public DateTime EmployeeDate { get; private set; }
        public bool IsTerminated { get; private set; }
        private List<PerformanceReview> performanceReviews; 
        public IReadOnlyList<PerformanceReview> PerformanceReviews => performanceReviews.AsReadOnly();

        public Employee(string name, int age, decimal salary, Department department  )
        {
            Id = nextId++;
            Name = name;
            Age = age;
            Salary = salary;
            Department = department;
            EmployeeDate = DateTime.Now;
            performanceReviews = new List<PerformanceReview>();

            // PerformanceRating = 0;
            department.AddEmployee(this);
        }
              public void Terminate()
        {
            if (IsTerminated)
            {
                Console.WriteLine($"Employee {Name} is terminated and cannot be transferred.");
                return;
            }

            IsTerminated = true;
            Department.RemoveEmployee(this);
            Console.WriteLine($"Employee {Name} has been terminated.");
        }
        public void TransferDepartment(Department newDepartment)
        {
            if (IsTerminated)
            {
                Console.WriteLine($"Employee {Name} is terminated and cannot be transferred.");
                return;

            }
            if(Department == newDepartment)
            {
                Console.WriteLine($"Employee {Name} is already in {newDepartment.Name}.");
                return;

            }
            Department.RemoveEmployee(this);
            newDepartment.AddEmployee(this);
            Department = newDepartment;
            Console.WriteLine($"Employee {Name} has been transferred to {Department.Name}.");
        }



    }
}

       
