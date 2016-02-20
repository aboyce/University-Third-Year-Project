namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserinfoonRecievedTextMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceivedTextMessages", "UserFromId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ReceivedTextMessages", "UserFromId");
            AddForeignKey("dbo.ReceivedTextMessages", "UserFromId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceivedTextMessages", "UserFromId", "dbo.Users");
            DropIndex("dbo.ReceivedTextMessages", new[] { "UserFromId" });
            DropColumn("dbo.ReceivedTextMessages", "UserFromId");
        }
    }
}
