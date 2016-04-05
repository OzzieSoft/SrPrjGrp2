using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CareerTracker.Models
{
    public class Artifact
    {
        [Key]
        public int ID { get; set; }
        public bool IsResume { get; set; }
		[Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "File Name")]
        public string Location { get; set; }

		[Display(Name= "Private?")]
		public bool Private { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

    }
}