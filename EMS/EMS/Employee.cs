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
        public static int nextId = 1;

        public int Id { get;  set; }
        public string Name { get; set; }
        public int Age { get;   set; }
        public decimal Salary { get; set; }
        public JobTitle Title { get; set; }
        public string DepartmentName { get; set; }
        public DateTime EmployeeDate { get; set; }
        public bool IsTerminated { get; set; }
      
        public List<PerformanceReview> performanceReviews { get; set; }
        //public IReadOnlyList<PerformanceReview> PerformanceReviews => performanceReviews.AsReadOnly();

        public Employee()
        {
            performanceReviews = new List<PerformanceReview>();
        }
        public Employee(string name, int age,JobTitle jobTitle, decimal salary, string  departmentName  )
        {
            Id = nextId++;
           
            Name = name;
            Age = age;
            Salary = salary;
            DepartmentName = departmentName;
            EmployeeDate = DateTime.Now;
            performanceReviews = new List<PerformanceReview>();
            Title = jobTitle;
            // PerformanceRating = 0;
            IsTerminated = false;
 
        }

        public double GetAveragePerformance()
        {
            if (performanceReviews.Count == 0) return 0;
            return performanceReviews.Average(pr => (int)pr.Rating);
        }


        public void Promote(JobTitle title, decimal percentage = 0)
        {
           
            
             Title = title;
             Salary += Salary * percentage / 100;
         }



        public override bool Equals(object obj)
        {
            if (obj is Employee other)
            {
                return this.Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public void Terminate()
        {
            

            IsTerminated = true;
            //Department.RemoveEmployee(this);

        }
        public void TransferDepartment(string departmentName)
        {
            if (IsTerminated) throw new Exception("Can't transfer terminated employee!");
            DepartmentName = departmentName;
        }

        public void AddPerformanceReview(PerformanceRating rating, string comments = "")
        {

            PerformanceReview oldPer = performanceReviews.FirstOrDefault(p => p.ReviewDate.Month ==DateTime.Now.Month);

            if (oldPer == null)
            {
                performanceReviews.Add(new PerformanceReview(rating, comments));

                string message = $"✅ {Name}'s performance review added successfully! Rating: {rating}";

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✓ {message}");
                Console.ResetColor();

            }
            else
            {
                oldPer.Rating = rating;
                oldPer.ReviewerComments = comments;
                string message = $"✅ {Name}'s performance review Edited successfully! Rating: {rating}";

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✓ {message}");
                Console.ResetColor();

            }
        }
     
        public bool IsEligibleForPromotion()
        {
            if (performanceReviews.Count < 4)
                return false;

            
            return GetAveragePerformance()>=4;
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





