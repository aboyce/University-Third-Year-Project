namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoredtheNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleNotifications", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.RoleNotifications", "Message", c => c.String());
            AddColumn("dbo.RoleNotifications", "NotificationAbout_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.RoleNotifications", "Role_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.UserNotifications", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.UserNotifications", "Message", c => c.String());
            AddColumn("dbo.UserNotifications", "NotificationAbout_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.RoleNotifications", "NotificationAbout_Id");
            CreateIndex("dbo.RoleNotifications", "Role_Id");
            CreateIndex("dbo.UserNotifications", "NotificationAbout_Id");
            AddForeignKey("dbo.RoleNotifications", "NotificationAbout_Id", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RoleNotifications", "Role_Id", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserNotifications", "NotificationAbout_Id", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.RoleNotifications", "RoleId");
            DropColumn("dbo.RoleNotifications", "UserIdNotificationOn");
            DropColumn("dbo.RoleNotifications", "NotificationType");
            DropColumn("dbo.RoleNotifications", "NotificationMessage");
            DropColumn("dbo.UserNotifications", "UserIdNotificationOn");
            DropColumn("dbo.UserNotifications", "NotificationType");
            DropColumn("dbo.UserNotifications", "NotificationMessage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserNotifications", "NotificationMessage", c => c.String());
            AddColumn("dbo.UserNotifications", "NotificationType", c => c.Int(nullable: false));
            AddColumn("dbo.UserNotifications", "UserIdNotificationOn", c => c.String(nullable: false));
            AddColumn("dbo.RoleNotifications", "NotificationMessage", c => c.String());
            AddColumn("dbo.RoleNotifications", "NotificationType", c => c.Int(nullable: false));
            AddColumn("dbo.RoleNotifications", "UserIdNotificationOn", c => c.String(nullable: false));
            AddColumn("dbo.RoleNotifications", "RoleId", c => c.String(nullable: false));
            DropForeignKey("dbo.UserNotifications", "NotificationAbout_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.RoleNotifications", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.RoleNotifications", "NotificationAbout_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.UserNotifications", new[] { "NotificationAbout_Id" });
            DropIndex("dbo.RoleNotifications", new[] { "Role_Id" });
            DropIndex("dbo.RoleNotifications", new[] { "NotificationAbout_Id" });
            DropColumn("dbo.UserNotifications", "NotificationAbout_Id");
            DropColumn("dbo.UserNotifications", "Message");
            DropColumn("dbo.UserNotifications", "Type");
            DropColumn("dbo.RoleNotifications", "Role_Id");
            DropColumn("dbo.RoleNotifications", "NotificationAbout_Id");
            DropColumn("dbo.RoleNotifications", "Message");
            DropColumn("dbo.RoleNotifications", "Type");
        }
    }
}
