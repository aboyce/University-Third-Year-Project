namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedUsertoUserExtra : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "UserExtras");
            RenameColumn(table: "dbo.Tickets", name: "UserAssignedToId", newName: "UserExtraAssignedToId");
            RenameColumn(table: "dbo.ApplicationUsers", name: "UserId", newName: "UserExtraId");
            RenameIndex(table: "dbo.Tickets", name: "IX_UserAssignedToId", newName: "IX_UserExtraAssignedToId");
            RenameIndex(table: "dbo.ApplicationUsers", name: "IX_UserId", newName: "IX_UserExtraId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ApplicationUsers", name: "IX_UserExtraId", newName: "IX_UserId");
            RenameIndex(table: "dbo.Tickets", name: "IX_UserExtraAssignedToId", newName: "IX_UserAssignedToId");
            RenameColumn(table: "dbo.ApplicationUsers", name: "UserExtraId", newName: "UserId");
            RenameColumn(table: "dbo.Tickets", name: "UserExtraAssignedToId", newName: "UserAssignedToId");
            RenameTable(name: "dbo.UserExtras", newName: "Users");
        }
    }
}
