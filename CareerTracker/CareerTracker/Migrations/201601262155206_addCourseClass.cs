namespace CareerTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCourseClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Course",
                c => new
                {
                    ID = c.Int(nullable:false, identity:true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.CourseUserProfile",
                c => new
                {
                    Course_ID = c.Int(nullable: false),
                    UserProfile_ID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Course_ID, t.UserProfile_ID })
                .ForeignKey("dbo.Course", t => t.Course_ID, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_ID, cascadeDelete: true)
                .Index(t => t.Course_ID)
                .Index(t => t.UserProfile_ID);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CourseUserProfile", new[] {"Course_ID"});
            DropIndex("dbo.CourseUserProfile", new[] {"UserProfile_ID"});
            DropForeignKey("dbo.CourseUserProfile", "Course_ID", "dbo.Course");
            DropForeignKey("dbo.CourseUserProfile", "UserProfile_ID", "dbo.UserProfile");
            DropTable("dbo.CourseUserProfile");
            DropTable("dbo.Course");
        }
    }
}
