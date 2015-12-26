namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInternalExternalboolonTicketLogType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLogs", "IsInternal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketLogs", "IsInternal");
        }
    }
}
