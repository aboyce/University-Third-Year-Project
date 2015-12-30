namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplituptextmessagesintoSentandReceived : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TextMessages", "UserTo_Id", "dbo.Users");
            DropIndex("dbo.TextMessages", new[] { "UserTo_Id" });
            CreateTable(
                "dbo.ReceivedTextMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClockworkId = c.String(nullable: false),
                        ClockworkNetworkCode = c.String(nullable: false),
                        ClockworkNetworkKeyword = c.String(nullable: false),
                        Read = c.Boolean(nullable: false),
                        Received = c.DateTime(nullable: false),
                        To = c.String(nullable: false),
                        From = c.String(nullable: false),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SentTextMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserToId = c.String(nullable: false, maxLength: 128),
                        Sent = c.DateTime(nullable: false),
                        To = c.String(nullable: false),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserToId, cascadeDelete: true)
                .Index(t => t.UserToId);
            
            DropTable("dbo.TextMessages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TextMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserToId = c.String(nullable: false),
                        Number = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        Sent = c.DateTime(nullable: false),
                        UserTo_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.SentTextMessages", "UserToId", "dbo.Users");
            DropIndex("dbo.SentTextMessages", new[] { "UserToId" });
            DropTable("dbo.SentTextMessages");
            DropTable("dbo.ReceivedTextMessages");
            CreateIndex("dbo.TextMessages", "UserTo_Id");
            AddForeignKey("dbo.TextMessages", "UserTo_Id", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
