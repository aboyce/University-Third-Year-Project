namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedtheEnitityTicketLogType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketLogs", "TicketLogTypeId", "dbo.TicketLogTypes");
            DropIndex("dbo.TicketLogs", new[] { "TicketLogTypeId" });
            AddColumn("dbo.TicketLogs", "TicketLogType", c => c.Int(nullable: false));
            DropColumn("dbo.TicketLogs", "TicketLogTypeId");
            DropTable("dbo.TicketLogTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TicketLogTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TicketLogs", "TicketLogTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.TicketLogs", "TicketLogType");
            CreateIndex("dbo.TicketLogs", "TicketLogTypeId");
            AddForeignKey("dbo.TicketLogs", "TicketLogTypeId", "dbo.TicketLogTypes", "Id", cascadeDelete: true);
        }
    }
}
