using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTracker.Models
{
    public class GoalStep
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }

        public virtual Goal Goal { get; set; }
    }
}