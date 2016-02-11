using CareerTracker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CareerTracker.DAL
{
    public class CTContext : IdentityDbContext<User>
    {
        public CTContext() : base("DefaultConnection", throwIfV1Schema: false) { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalStep> GoalSteps { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Category> Categories { get; set; }

        
    }
}