using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OzzieSoftCareerTracker.Models
{
    public class Goal
    {
        public int goalID { get; set; }
        public string goalTitle { get; set; }
        public string goalDesc { get; set; }
        public DateTime goalDueDate { get; set; }

        public virtual ICollection<Skill> skills { get; set; }
        public virtual UserProfile user {get; set; }
    }
}