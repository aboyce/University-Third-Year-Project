namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverridePhoneNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationUsers", "PhoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationUsers", "PhoneNumber", c => c.String());
        }
    }
}
