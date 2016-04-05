using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CareerTracker.Models
{
    public class Goal
    {
        [Key]
        public int ID { get; set; }
		[Required]
        public string Name { get; set; }
        public string Description { get; set; }
		
		[Display(Name="Private?")]
		public bool Private { get; set; }

		[Display(Name="Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DueDate { get; set; }

        public virtual ICollection<GoalStep> Steps { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}