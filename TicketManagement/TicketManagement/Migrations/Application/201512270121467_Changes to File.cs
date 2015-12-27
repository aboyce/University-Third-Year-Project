namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangestoFile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketLogs", "FileId", "dbo.Files");
            DropIndex("dbo.TicketLogs", new[] { "FileId" });
            AlterColumn("dbo.TicketLogs", "FileId", c => c.Int());
            CreateIndex("dbo.TicketLogs", "FileId");
            AddForeignKey("dbo.TicketLogs", "FileId", "dbo.Files", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLogs", "FileId", "dbo.Files");
            DropIndex("dbo.TicketLogs", new[] { "FileId" });
            AlterColumn("dbo.TicketLogs", "FileId", c => c.Int(nullable: false));
            CreateIndex("dbo.TicketLogs", "FileId");
            AddForeignKey("dbo.TicketLogs", "FileId", "dbo.Files", "Id", cascadeDelete: true);
        }
    }
}
