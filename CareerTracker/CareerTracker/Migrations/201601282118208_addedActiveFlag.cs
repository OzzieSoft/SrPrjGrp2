namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedActiveFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "active");
        }
    }
}
