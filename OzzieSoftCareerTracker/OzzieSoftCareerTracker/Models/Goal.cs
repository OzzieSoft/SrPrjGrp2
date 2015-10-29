using System;

namespace OzzieSoftCareerTracker.Models
{
    public class Goal
    {
        public int goalID { get; set; }
        public string goalName { get; set; }
        public DateTime goalDueDate { get; set; }
        public string goalDesc { get; set; }
    }
}
