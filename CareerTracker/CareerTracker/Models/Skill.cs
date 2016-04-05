using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CareerTracker.Models
{
    public class Skill
    {
        [Key]
        public int ID { get; set; }
        public Type Type { get; set; }
		[Required]
        public string Name { get; set; }
        public string Description { get; set; }

		[Display(Name = "Private?")]
		public bool Private { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }

    public enum Type { job, course, volunteering }
}