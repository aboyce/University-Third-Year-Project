namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEntityBaseClasstoUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "UserName");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "Telephone");
            DropColumn("dbo.Users", "Created");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Telephone", c => c.String(maxLength: 50));
            AddColumn("dbo.Users", "Email", c => c.String(maxLength: 50));
            AddColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
