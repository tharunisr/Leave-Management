namespace LeaveManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailrequiredremoved : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Leaves", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leaves", "Email", c => c.String(nullable: false));
        }
    }
}
