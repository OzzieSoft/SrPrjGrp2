using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CareerTracker.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<Artifact> Artifacts { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}