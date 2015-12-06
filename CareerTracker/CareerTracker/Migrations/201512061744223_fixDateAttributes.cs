namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixDateAttributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Goals", "DueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.GoalSteps", "DueDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GoalSteps", "DueDate", c => c.String());
            AlterColumn("dbo.Goals", "DueDate", c => c.String());
        }
    }
}
