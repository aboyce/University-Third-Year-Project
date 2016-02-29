namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddeddeliveryreceiptinformationtoSentTextMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SentTextMessages", "DeliveryStatus", c => c.String());
            AddColumn("dbo.SentTextMessages", "DeliveryDetail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SentTextMessages", "DeliveryDetail");
            DropColumn("dbo.SentTextMessages", "DeliveryStatus");
        }
    }
}
