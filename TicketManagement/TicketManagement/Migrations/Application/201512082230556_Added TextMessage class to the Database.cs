namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTextMessageclasstotheDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TextMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserToId = c.String(nullable: false, maxLength: 128),
                        Number = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        Sent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserToId, cascadeDelete: true)
                .Index(t => t.ApplicationUserToId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextMessages", "ApplicationUserToId", "dbo.ApplicationUsers");
            DropIndex("dbo.TextMessages", new[] { "ApplicationUserToId" });
            DropTable("dbo.TextMessages");
        }
    }
}
