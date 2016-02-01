namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CategoryGoals", newName: "GoalCategories");
            RenameTable(name: "dbo.ArtifactCategories", newName: "CategoryArtifacts");
            DropPrimaryKey("dbo.GoalCategories");
            DropPrimaryKey("dbo.CategoryArtifacts");
            AddPrimaryKey("dbo.GoalCategories", new[] { "Goal_ID", "Category_ID" });
            AddPrimaryKey("dbo.CategoryArtifacts", new[] { "Category_ID", "Artifact_ID" });
            CreateIndex("dbo.Artifacts", "User_UserId");
            CreateIndex("dbo.Goals", "User_UserId");
            CreateIndex("dbo.GoalSteps", "Goal_ID");
            CreateIndex("dbo.Skills", "User_UserId");
            CreateIndex("dbo.CategoryArtifacts", "Category_ID");
            CreateIndex("dbo.CategoryArtifacts", "Artifact_ID");
            CreateIndex("dbo.GoalCategories", "Goal_ID");
            CreateIndex("dbo.GoalCategories", "Category_ID");
            CreateIndex("dbo.CourseUserProfiles", "Course_ID");
            CreateIndex("dbo.CourseUserProfiles", "UserProfile_UserId");
            CreateIndex("dbo.SkillCategories", "Skill_ID");
            CreateIndex("dbo.SkillCategories", "Category_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SkillCategories", new[] { "Category_ID" });
            DropIndex("dbo.SkillCategories", new[] { "Skill_ID" });
            DropIndex("dbo.CourseUserProfiles", new[] { "UserProfile_UserId" });
            DropIndex("dbo.CourseUserProfiles", new[] { "Course_ID" });
            DropIndex("dbo.GoalCategories", new[] { "Category_ID" });
            DropIndex("dbo.GoalCategories", new[] { "Goal_ID" });
            DropIndex("dbo.CategoryArtifacts", new[] { "Artifact_ID" });
            DropIndex("dbo.CategoryArtifacts", new[] { "Category_ID" });
            DropIndex("dbo.Skills", new[] { "User_UserId" });
            DropIndex("dbo.GoalSteps", new[] { "Goal_ID" });
            DropIndex("dbo.Goals", new[] { "User_UserId" });
            DropIndex("dbo.Artifacts", new[] { "User_UserId" });
            DropPrimaryKey("dbo.CategoryArtifacts");
            DropPrimaryKey("dbo.GoalCategories");
            AddPrimaryKey("dbo.CategoryArtifacts", new[] { "Artifact_ID", "Category_ID" });
            AddPrimaryKey("dbo.GoalCategories", new[] { "Category_ID", "Goal_ID" });
            RenameTable(name: "dbo.CategoryArtifacts", newName: "ArtifactCategories");
            RenameTable(name: "dbo.GoalCategories", newName: "CategoryGoals");
        }
    }
}
