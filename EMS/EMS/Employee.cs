using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EMS
{
    public enum JobTitle
    {
        Fresh = 0,
        Junior = 1,
        MidLevel = 2,
        Senior = 3,
        Manager = 4,
        Director = 5
    }

    internal class Employee
    {  
        private static int nextId = 1;

        public int Id { get;private set; }
        public string Name { get;private set; }
        public int Age { get; private set; }
        public double Salary { get; private set; }
        public JobTitle Title { get; private set; }
        public Department Department { get; private set; }
        public DateTime EmployeeDate { get; private set; }
        public bool IsTerminated { get; private set; }
      
        private List<PerformanceReview> performanceReviews; 
        public IReadOnlyList<PerformanceReview> PerformanceReviews => performanceReviews.AsReadOnly();

        public Employee(string name, int age, double salary, Department department  )
        {
            Id = nextId++;
            Name = name;
            Age = age;
            Salary = salary;
            Department = department;
            EmployeeDate = DateTime.Now;
            performanceReviews = new List<PerformanceReview>();
            Title = JobTitle.Fresh;
            // PerformanceRating = 0;
            IsTerminated = false;
            department.AddEmployee(this);
        }
        public void Terminate()
        {

            IsTerminated = true;
            //Department.RemoveEmployee(this);
       
        }
        public void TransferDepartment(Department newDepartment)
        {
            Department.RemoveEmployee(this);
            newDepartment.AddEmployee(this);
            Department = newDepartment;
           
        }

        public void AddPerformanceReview(PerformanceRating rating, string comments = "")
        {
            int Month = EmployeeDate.Month;
            int NMonth = DateTime.Now.Month;
            if(NMonth!= 1 && NMonth!=4 && NMonth !=7 && NMonth !=10)
            {
                Console.WriteLine(" This is NOT the  a quarter !");
                return;
            }
            if (performanceReviews.Count == 3)
            {
                performanceReviews.Add(new PerformanceReview(rating, comments));
                Console.WriteLine($"{Name}'s performance review added successfully! ✅");
                
             
                return;
            }
            
              performanceReviews.Add(new PerformanceReview(rating, comments));
           

            Console.WriteLine($"{Name}'s performance review added successfully! ✅");
        }
        


        
        
        
        public bool IsEligibleForPromotion()
        {
            if (performanceReviews.Count < 4)
                return false;

            //int highRatings = 0;
            //foreach (var review in performanceReviews)
            //{
            //    if (review.Rating == PerformanceRating.Excellent) highRatings++;

            //}

            //return highRatings >= 3;
            return GetAveragePerformance()>=4.5;
        }


        public void Promote(JobTitle title , double percentage=0)
        {
            //if (!IsEligibleForPromotion())
            //{
            //    Console.WriteLine($"{Name} is NOT eligible for promotion 🚫");
            //    return;
            //}

            //if (Title == JobTitle.Director)
            //{
            //    Console.WriteLine($"❌ {Name} is already at the highest level ({Title}). No further promotions possible.");
            //    return;
            //}

            // Title++;
            Title = title;
           //  double increasePercentage = JobTitleSalaryIncrease[title];
            Salary += Salary * percentage / 100;

            Console.WriteLine($"🎉 {Name} has been promoted to {Title} with a new salary of {Salary:C}! 🚀");
        }

        public double GetAveragePerformance()
        {
            if (performanceReviews.Count == 0)
            {
                return 0;
            }

            double sum = 0;
            foreach (var review in performanceReviews)
            {
                sum += (int)review.Rating;
            }

            return sum / performanceReviews.Count;
        }

        private static readonly Dictionary<JobTitle, double> JobTitleSalaryIncrease = new Dictionary<JobTitle, double>
{
    { JobTitle.Fresh, 5 },   
      { JobTitle.Junior, 10 },      
    { JobTitle.MidLevel, 15 },    
    { JobTitle.Senior, 20 },      
    { JobTitle.Manager, 25 },     
    { JobTitle.Director, 30 }      
};

    }



}





