namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedaRoleIdtotheRoleNotification : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.RoleNotifications", name: "Role_Id", newName: "RoleId");
            RenameIndex(table: "dbo.RoleNotifications", name: "IX_Role_Id", newName: "IX_RoleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.RoleNotifications", name: "IX_RoleId", newName: "IX_Role_Id");
            RenameColumn(table: "dbo.RoleNotifications", name: "RoleId", newName: "Role_Id");
        }
    }
}
