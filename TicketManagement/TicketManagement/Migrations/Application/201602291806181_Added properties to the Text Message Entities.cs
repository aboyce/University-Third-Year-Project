namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedpropertiestotheTextMessageEntities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SentTextMessages", "Success", c => c.Boolean(nullable: false));
            AddColumn("dbo.SentTextMessages", "ErrorCode", c => c.Int());
            AddColumn("dbo.SentTextMessages", "ErrorMessage", c => c.String());
            AddColumn("dbo.SentTextMessages", "ClockworkId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SentTextMessages", "ClockworkId");
            DropColumn("dbo.SentTextMessages", "ErrorMessage");
            DropColumn("dbo.SentTextMessages", "ErrorCode");
            DropColumn("dbo.SentTextMessages", "Success");
        }
    }
}
