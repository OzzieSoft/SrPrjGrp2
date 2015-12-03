using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTracker.Models
{
    public class CombinedViewModel
    {
        public Artifact ArtifactModel { get; set; }
        public Skill SkillModel { get; set; }
        public Goal GoalModel { get; set; }
    }
}