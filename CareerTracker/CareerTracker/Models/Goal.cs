using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CareerTracker.Models
{
    public class Goal
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string DueDate { get; set; }
        public string Description { get; set; }

        public virtual ICollection<GoalStep> Steps { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}