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
		[Required]
        public string Name { get; set; }
        public string Description { get; set; }

		[Display(Name = "Private?")]
		public bool Private { get; set; }

		[Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/dd/yyyy}")]
        public DateTime DueDate { get; set; }

        public virtual Goal Goal { get; set; }
    }
}