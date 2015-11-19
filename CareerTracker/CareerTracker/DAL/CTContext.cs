using CareerTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CareerTracker.DAL
{
    public class CTContext : DbContext
    {
        public CTContext() : base("DefaultConnection") { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalStep> GoalSteps { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}