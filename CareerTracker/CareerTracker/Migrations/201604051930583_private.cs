namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _private : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artifacts", "Private", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goals", "Private", c => c.Boolean(nullable: false));
            AddColumn("dbo.GoalSteps", "Privatee", c => c.Boolean(nullable: false));
            AddColumn("dbo.Skills", "Private", c => c.Boolean(nullable: false));
            DropColumn("dbo.Artifacts", "Visible");
            DropColumn("dbo.Goals", "Visible");
            DropColumn("dbo.GoalSteps", "Visible");
            DropColumn("dbo.Skills", "Visible");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Skills", "Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.GoalSteps", "Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goals", "Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Artifacts", "Visible", c => c.Boolean(nullable: false));
            DropColumn("dbo.Skills", "Private");
            DropColumn("dbo.GoalSteps", "Privatee");
            DropColumn("dbo.Goals", "Private");
            DropColumn("dbo.Artifacts", "Private");
        }
    }
}
