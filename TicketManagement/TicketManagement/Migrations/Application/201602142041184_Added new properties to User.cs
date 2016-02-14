namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddednewpropertiestoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "MobileApplicationConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "UserToken", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UserToken");
            DropColumn("dbo.Users", "MobileApplicationConfirmed");
        }
    }
}
