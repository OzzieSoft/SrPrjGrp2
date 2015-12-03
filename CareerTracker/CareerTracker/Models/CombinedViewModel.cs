using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CareerTracker.Models
{
    public class CombinedViewModel
    {
        public Artifact Artifact { get; set; }
        public Goal Goal { get; set; }
        public Skill Skill { get; set; }
    }
}