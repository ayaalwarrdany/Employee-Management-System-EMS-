using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS
{
   
    class PerformanceReview
    {
        public DateTime ReviewDate { get; private set; }
        public double Rating { get; private set; }
        public string ReviewerComments { get; private set; }

        public PerformanceReview(double rating, string comments = "")
        {
           
             Rating = rating;
            ReviewerComments = comments;
            ReviewDate = DateTime.Now;
        }
       
    }
}
