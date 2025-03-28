using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS
{
    public enum PerformanceRating
    {
        Poor = 1,       
        Average = 2,    
        Good = 3,        
        Excellent = 4,  
        Outstanding = 5  
    }


    class PerformanceReview
    {
        public DateTime ReviewDate { get; set; }
        public PerformanceRating Rating { get; set; }
        public string ReviewerComments { get;set; }

        public PerformanceReview(PerformanceRating rating, string comments = "")
        {
           
             Rating = rating;
            ReviewerComments = comments;
            ReviewDate = DateTime.Now;
        }
       
    }
}
