namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedaUserIdtotheNotificationBase : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.RoleNotifications", name: "NotificationAbout_Id", newName: "NotificationAboutId");
            RenameColumn(table: "dbo.UserNotifications", name: "NotificationAbout_Id", newName: "NotificationAboutId");
            RenameIndex(table: "dbo.RoleNotifications", name: "IX_NotificationAbout_Id", newName: "IX_NotificationAboutId");
            RenameIndex(table: "dbo.UserNotifications", name: "IX_NotificationAbout_Id", newName: "IX_NotificationAboutId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserNotifications", name: "IX_NotificationAboutId", newName: "IX_NotificationAbout_Id");
            RenameIndex(table: "dbo.RoleNotifications", name: "IX_NotificationAboutId", newName: "IX_NotificationAbout_Id");
            RenameColumn(table: "dbo.UserNotifications", name: "NotificationAboutId", newName: "NotificationAbout_Id");
            RenameColumn(table: "dbo.RoleNotifications", name: "NotificationAboutId", newName: "NotificationAbout_Id");
        }
    }
}
