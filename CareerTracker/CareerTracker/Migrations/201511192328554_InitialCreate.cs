namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        email = c.String(),
                        lastName = c.String(),
                        firstName = c.String(),
                        dateOfBirth = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DueDate = c.String(),
                        Description = c.String(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.GoalSteps",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DueDate = c.String(),
                        Goal_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Goals", t => t.Goal_ID)
                .Index(t => t.Goal_ID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Goal_ID = c.Int(),
                        Artifact_ID = c.Int(),
                        Skill_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Goals", t => t.Goal_ID)
                .ForeignKey("dbo.Artifacts", t => t.Artifact_ID)
                .ForeignKey("dbo.Skills", t => t.Skill_ID)
                .Index(t => t.Goal_ID)
                .Index(t => t.Artifact_ID)
                .Index(t => t.Skill_ID);
            
            CreateTable(
                "dbo.Artifacts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsResume = c.Boolean(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Skills", new[] { "User_UserId" });
            DropIndex("dbo.Artifacts", new[] { "User_UserId" });
            DropIndex("dbo.Categories", new[] { "Skill_ID" });
            DropIndex("dbo.Categories", new[] { "Artifact_ID" });
            DropIndex("dbo.Categories", new[] { "Goal_ID" });
            DropIndex("dbo.GoalSteps", new[] { "Goal_ID" });
            DropIndex("dbo.Goals", new[] { "User_UserId" });
            DropForeignKey("dbo.Skills", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Artifacts", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Categories", "Skill_ID", "dbo.Skills");
            DropForeignKey("dbo.Categories", "Artifact_ID", "dbo.Artifacts");
            DropForeignKey("dbo.Categories", "Goal_ID", "dbo.Goals");
            DropForeignKey("dbo.GoalSteps", "Goal_ID", "dbo.Goals");
            DropForeignKey("dbo.Goals", "User_UserId", "dbo.UserProfile");
            DropTable("dbo.Skills");
            DropTable("dbo.Artifacts");
            DropTable("dbo.Categories");
            DropTable("dbo.GoalSteps");
            DropTable("dbo.Goals");
            DropTable("dbo.UserProfile");
        }
    }
}
