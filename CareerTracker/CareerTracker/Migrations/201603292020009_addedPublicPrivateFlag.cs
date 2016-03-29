namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPublicPrivateFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artifacts", "Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goals", "Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.GoalSteps", "Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Skills", "Visible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skills", "Visible");
            DropColumn("dbo.GoalSteps", "Visible");
            DropColumn("dbo.Goals", "Visible");
            DropColumn("dbo.Artifacts", "Visible");
        }
    }
}
