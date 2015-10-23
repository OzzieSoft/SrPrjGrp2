using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace OzzieSoftCareerTracker.Models
{
    public class Artifact
    {
		public int ArtifactID { get; set; }
		public string ArtifactName { get; set; }
		public string ArtifactDescription { get; set; }
    }

	public class ArtifactDBContext : DbContext{
		public DbSet<Artifact> Artifacts { get; set; }
	}
}