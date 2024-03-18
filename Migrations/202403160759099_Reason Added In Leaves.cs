namespace LeaveManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReasonAddedInLeaves : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leaves", "Reason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leaves", "Reason");
        }
    }
}
