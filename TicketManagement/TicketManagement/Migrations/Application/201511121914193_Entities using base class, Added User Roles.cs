namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntitiesusingbaseclassAddedUserRoles : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Organisations", "Created");
            DropColumn("dbo.Users", "IsInternal");
            DropColumn("dbo.Users", "IsAdmin");
            DropColumn("dbo.Teams", "Created");
            DropColumn("dbo.Projects", "Created");
            DropColumn("dbo.TicketCategories", "Created");
            DropColumn("dbo.Tickets", "Created");
            DropColumn("dbo.TicketPriorities", "Created");
            DropColumn("dbo.TicketStates", "Created");
            DropColumn("dbo.TicketLogTypes", "Created");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketLogTypes", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketStates", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketPriorities", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tickets", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketCategories", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Teams", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "IsAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "IsInternal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organisations", "Created", c => c.DateTime(nullable: false));
        }
    }
}
