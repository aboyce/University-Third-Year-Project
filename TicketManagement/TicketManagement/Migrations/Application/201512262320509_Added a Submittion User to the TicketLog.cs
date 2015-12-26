namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedaSubmittionUsertotheTicketLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLogs", "SubmittedByUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TicketLogs", "SubmittedByUserId");
            AddForeignKey("dbo.TicketLogs", "SubmittedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLogs", "SubmittedByUserId", "dbo.Users");
            DropIndex("dbo.TicketLogs", new[] { "SubmittedByUserId" });
            DropColumn("dbo.TicketLogs", "SubmittedByUserId");
        }
    }
}
