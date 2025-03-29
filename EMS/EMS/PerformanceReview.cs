using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization; // Add this for JsonConstructor

namespace EMS
{
    public enum PerformanceRating
    {
        Poor = 1,
        Fair = 2,
        Good = 3,
        VeryGood=4,
        Excellent =5
        
    }

    class PerformanceReview
    {
        public DateTime ReviewDate { get; set; }
        public PerformanceRating Rating { get; set; }
        public string ReviewerComments { get; set; }

        // Parameterless constructor required for deserialization
        public PerformanceReview()
        {
            ReviewDate = DateTime.Now; // Default value
            Rating = PerformanceRating.Poor; // Default value
            ReviewerComments = ""; // Default value
        }

        // Constructor for deserialization, marked with JsonConstructor
        [JsonConstructor]
        public PerformanceReview(DateTime reviewDate, PerformanceRating rating, string reviewerComments)
        {
            ReviewDate = reviewDate;
            Rating = rating;
            ReviewerComments = reviewerComments;
        }

        // Original constructor for adding new reviews
        public PerformanceReview(PerformanceRating rating, string comments = "")
        {
            Rating = rating;
            ReviewerComments = comments;
            ReviewDate = DateTime.Now;
        }
    }
}