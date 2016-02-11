namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrateToClaims : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.CategoryGoals", newName: "GoalCategories");
            //RenameTable(name: "dbo.ArtifactCategories", newName: "CategoryArtifacts");
            //DropPrimaryKey("dbo.CategoryGoals");
            //DropPrimaryKey("dbo.ArtifactCategories");
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false, maxLength: 256),
                        lastName = c.String(),
                        firstName = c.String(),
                        active = c.Boolean(nullable: false),
                        dateOfBirth = c.DateTime(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        MobileAlias = c.String(),
                        IsAnonymous = c.Boolean(nullable: false),
                        LastActivityDate = c.DateTime(nullable: false),
                        MobilePIN = c.String(),
                        LoweredEmail = c.String(),
                        LoweredUserName = c.String(),
                        PasswordQuestion = c.String(),
                        PasswordAnswer = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        IsLockedOut = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                        LastPasswordChangedDate = c.DateTime(nullable: false),
                        LastLockoutDate = c.DateTime(nullable: false),
                        FailedPasswordAttemptCount = c.Int(nullable: false),
                        FailedPasswordAttemptWindowStart = c.DateTime(nullable: false),
                        FailedPasswordAnswerAttemptCount = c.Int(nullable: false),
                        FailedPasswordAnswerAttemptWindowStart = c.DateTime(nullable: false),
                        Comment = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Goals", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Artifacts", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Skills", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Courses", "User_Id", c => c.String(maxLength: 128));
            //AddPrimaryKey("dbo.GoalCategories", new[] { "Goal_ID", "Category_ID" });
            //AddPrimaryKey("dbo.CategoryArtifacts", new[] { "Category_ID", "Artifact_ID" });
            //CreateIndex("dbo.Artifacts", "User_UserId");
            //CreateIndex("dbo.Artifacts", "User_Id");
            //CreateIndex("dbo.Goals", "User_UserId");
            //CreateIndex("dbo.Goals", "User_Id");
            //CreateIndex("dbo.GoalSteps", "Goal_ID");
            //CreateIndex("dbo.Courses", "User_Id");
            //CreateIndex("dbo.Skills", "User_UserId");
            //CreateIndex("dbo.Skills", "User_Id");
            //CreateIndex("dbo.CategoryArtifacts", "Category_ID");
            //CreateIndex("dbo.CategoryArtifacts", "Artifact_ID");
            //CreateIndex("dbo.GoalCategories", "Goal_ID");
            //CreateIndex("dbo.GoalCategories", "Category_ID");
            //CreateIndex("dbo.CourseUserProfiles", "Course_ID");
            //CreateIndex("dbo.CourseUserProfiles", "UserProfile_UserId");
            //CreateIndex("dbo.SkillCategories", "Skill_ID");
            //CreateIndex("dbo.SkillCategories", "Category_ID");
            AddForeignKey("dbo.Artifacts", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Courses", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Goals", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Skills", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skills", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Goals", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Artifacts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            //DropIndex("dbo.SkillCategories", new[] { "Category_ID" });
            //DropIndex("dbo.SkillCategories", new[] { "Skill_ID" });
            //DropIndex("dbo.CourseUserProfiles", new[] { "UserProfile_UserId" });
            //DropIndex("dbo.CourseUserProfiles", new[] { "Course_ID" });
            //DropIndex("dbo.GoalCategories", new[] { "Category_ID" });
            //DropIndex("dbo.GoalCategories", new[] { "Goal_ID" });
            //DropIndex("dbo.CategoryArtifacts", new[] { "Artifact_ID" });
            //DropIndex("dbo.CategoryArtifacts", new[] { "Category_ID" });
            //DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            //DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            //DropIndex("dbo.AspNetUsers", "UserNameIndex");
            //DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            //DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            //DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            //DropIndex("dbo.Skills", new[] { "User_Id" });
            //DropIndex("dbo.Skills", new[] { "User_UserId" });
            //DropIndex("dbo.Courses", new[] { "User_Id" });
            //DropIndex("dbo.GoalSteps", new[] { "Goal_ID" });
            //DropIndex("dbo.Goals", new[] { "User_Id" });
            //DropIndex("dbo.Goals", new[] { "User_UserId" });
            //DropIndex("dbo.Artifacts", new[] { "User_Id" });
            //DropIndex("dbo.Artifacts", new[] { "User_UserId" });
            //DropPrimaryKey("dbo.CategoryArtifacts");
            //DropPrimaryKey("dbo.GoalCategories");
            DropColumn("dbo.Courses", "User_Id");
            DropColumn("dbo.Skills", "User_Id");
            DropColumn("dbo.Artifacts", "User_Id");
            DropColumn("dbo.Goals", "User_Id");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            //AddPrimaryKey("dbo.ArtifactCategories", new[] { "Artifact_ID", "Category_ID" });
            //AddPrimaryKey("dbo.CategoryGoals", new[] { "Category_ID", "Goal_ID" });
            //RenameTable(name: "dbo.CategoryArtifacts", newName: "ArtifactCategories");
            //RenameTable(name: "dbo.GoalCategories", newName: "CategoryGoals");
        }
    }
}
