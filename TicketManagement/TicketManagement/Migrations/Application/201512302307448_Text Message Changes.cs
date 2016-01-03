namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TextMessageChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceivedTextMessages", "ClockworkKeyword", c => c.String(nullable: false));
            DropColumn("dbo.ReceivedTextMessages", "ClockworkNetworkKeyword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReceivedTextMessages", "ClockworkNetworkKeyword", c => c.String(nullable: false));
            DropColumn("dbo.ReceivedTextMessages", "ClockworkKeyword");
        }
    }
}
