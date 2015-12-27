namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangestoTicket : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "Ticket_Id", "dbo.Tickets");
            DropIndex("dbo.Files", new[] { "Ticket_Id" });
            DropColumn("dbo.Files", "Ticket_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "Ticket_Id", c => c.Int());
            CreateIndex("dbo.Files", "Ticket_Id");
            AddForeignKey("dbo.Files", "Ticket_Id", "dbo.Tickets", "Id");
        }
    }
}
