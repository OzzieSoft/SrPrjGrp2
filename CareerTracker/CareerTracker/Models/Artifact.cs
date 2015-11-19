using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CareerTracker.Models
{
    public class Artifact
    {
        [Key]
        public int ID { get; set; }
        public bool IsResume { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}