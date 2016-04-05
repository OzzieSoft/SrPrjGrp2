namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoalSteps", "Private", c => c.Boolean(nullable: false));
            DropColumn("dbo.GoalSteps", "Privatee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoalSteps", "Privatee", c => c.Boolean(nullable: false));
            DropColumn("dbo.GoalSteps", "Private");
        }
    }
}
