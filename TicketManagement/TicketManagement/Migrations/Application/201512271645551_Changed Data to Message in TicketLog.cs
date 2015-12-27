namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDatatoMessageinTicketLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLogs", "Message", c => c.String());
            DropColumn("dbo.TicketLogs", "Data");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketLogs", "Data", c => c.String(nullable: false));
            DropColumn("dbo.TicketLogs", "Message");
        }
    }
}
