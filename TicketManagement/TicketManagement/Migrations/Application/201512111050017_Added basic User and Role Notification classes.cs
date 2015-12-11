namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedbasicUserandRoleNotificationclasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.String(nullable: false),
                        UserIdNotificationOn = c.String(nullable: false),
                        NotificationType = c.Int(nullable: false),
                        NotificationMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserIdNotificationOn = c.String(nullable: false),
                        NotificationType = c.Int(nullable: false),
                        NotificationMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserNotifications");
            DropTable("dbo.RoleNotifications");
        }
    }
}
