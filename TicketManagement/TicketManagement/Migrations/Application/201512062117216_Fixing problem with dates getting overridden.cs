namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixingproblemwithdatesgettingoverridden : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organisations", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserExtras", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Teams", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketCategories", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tickets", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketPriorities", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketStates", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketLogTypes", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketLogTypes", "Created");
            DropColumn("dbo.TicketStates", "Created");
            DropColumn("dbo.TicketPriorities", "Created");
            DropColumn("dbo.Tickets", "Created");
            DropColumn("dbo.TicketCategories", "Created");
            DropColumn("dbo.Projects", "Created");
            DropColumn("dbo.Teams", "Created");
            DropColumn("dbo.UserExtras", "Created");
            DropColumn("dbo.Organisations", "Created");
        }
    }
}
