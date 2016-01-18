namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixCategoryForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Goal_ID", "dbo.Goals");
            DropForeignKey("dbo.Categories", "Artifact_ID", "dbo.Artifacts");
            DropForeignKey("dbo.Categories", "Skill_ID", "dbo.Skills");
            DropIndex("dbo.Categories", new[] { "Goal_ID" });
            DropIndex("dbo.Categories", new[] { "Artifact_ID" });
            DropIndex("dbo.Categories", new[] { "Skill_ID" });
            CreateTable(
                "dbo.CategoryGoals",
                c => new
                    {
                        Category_ID = c.Int(nullable: false),
                        Goal_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_ID, t.Goal_ID })
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .ForeignKey("dbo.Goals", t => t.Goal_ID, cascadeDelete: true)
                .Index(t => t.Category_ID)
                .Index(t => t.Goal_ID);
            
            CreateTable(
                "dbo.ArtifactCategories",
                c => new
                    {
                        Artifact_ID = c.Int(nullable: false),
                        Category_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Artifact_ID, t.Category_ID })
                .ForeignKey("dbo.Artifacts", t => t.Artifact_ID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .Index(t => t.Artifact_ID)
                .Index(t => t.Category_ID);
            
            CreateTable(
                "dbo.SkillCategories",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        Category_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.Category_ID })
                .ForeignKey("dbo.Skills", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.Category_ID);
            
            DropColumn("dbo.Categories", "Goal_ID");
            DropColumn("dbo.Categories", "Artifact_ID");
            DropColumn("dbo.Categories", "Skill_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Skill_ID", c => c.Int());
            AddColumn("dbo.Categories", "Artifact_ID", c => c.Int());
            AddColumn("dbo.Categories", "Goal_ID", c => c.Int());
            DropIndex("dbo.SkillCategories", new[] { "Category_ID" });
            DropIndex("dbo.SkillCategories", new[] { "Skill_ID" });
            DropIndex("dbo.ArtifactCategories", new[] { "Category_ID" });
            DropIndex("dbo.ArtifactCategories", new[] { "Artifact_ID" });
            DropIndex("dbo.CategoryGoals", new[] { "Goal_ID" });
            DropIndex("dbo.CategoryGoals", new[] { "Category_ID" });
            DropForeignKey("dbo.SkillCategories", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.SkillCategories", "Skill_ID", "dbo.Skills");
            DropForeignKey("dbo.ArtifactCategories", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.ArtifactCategories", "Artifact_ID", "dbo.Artifacts");
            DropForeignKey("dbo.CategoryGoals", "Goal_ID", "dbo.Goals");
            DropForeignKey("dbo.CategoryGoals", "Category_ID", "dbo.Categories");
            DropTable("dbo.SkillCategories");
            DropTable("dbo.ArtifactCategories");
            DropTable("dbo.CategoryGoals");
            CreateIndex("dbo.Categories", "Skill_ID");
            CreateIndex("dbo.Categories", "Artifact_ID");
            CreateIndex("dbo.Categories", "Goal_ID");
            AddForeignKey("dbo.Categories", "Skill_ID", "dbo.Skills", "ID");
            AddForeignKey("dbo.Categories", "Artifact_ID", "dbo.Artifacts", "ID");
            AddForeignKey("dbo.Categories", "Goal_ID", "dbo.Goals", "ID");
        }
    }
}
