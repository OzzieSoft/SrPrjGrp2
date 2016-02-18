namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class urlToLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artifacts", "Location", c => c.String());
            DropColumn("dbo.Artifacts", "Url");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Artifacts", "Url", c => c.String());
            DropColumn("dbo.Artifacts", "Location");
        }
    }
}
