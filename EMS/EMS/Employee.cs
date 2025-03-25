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
        public int PerformanceRating { get; private set; }

        public Employee(string name, int age, decimal salary, Department department  )
        {
            Id = nextId++;
            Name = name;
            Age = age;
            Salary = salary;
            Department = department;
            EmployeeDate = DateTime.Now;
            PerformanceRating = 0;
            department.AddEmployee(this);



        }
        public void Terminate()
        {
            IsTerminated = true;
            Department.RemoveEmployee(this);
            Console.WriteLine($"Employee {Name} has been terminated.");
        }
        public void Promote(string newTitle, decimal increaseAmount)
        {
            if (IsTerminated)
            {
                Console.WriteLine($"Cannot promote terminated employee {Name}.");
               
            }

            Salary += increaseAmount;
            Console.WriteLine($"Employee {Name} promoted with salary increase: {increaseAmount:F}.");
        }
        public void UpdatePerformanceRating(int rating)
        {
            if (rating < 1 || rating > 5)
            {
                Console.WriteLine("Rating must be between 1 and 5.");
                return;
            }

            PerformanceRating = rating;
            Console.WriteLine($"Performance rating for {Name} updated to {rating}.");
        }


    }
}
