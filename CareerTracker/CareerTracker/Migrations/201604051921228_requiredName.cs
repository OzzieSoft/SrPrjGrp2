namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Goals", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.GoalSteps", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Skills", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Skills", "Name", c => c.String());
            AlterColumn("dbo.GoalSteps", "Name", c => c.String());
            AlterColumn("dbo.Goals", "Name", c => c.String());
        }
    }
}
