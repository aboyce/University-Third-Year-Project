namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organisations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsInternal = c.Boolean(nullable: false),
                        DefaultContactId = c.String(maxLength: 128),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DefaultContactId)
                .Index(t => t.DefaultContactId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        IsArchived = c.Boolean(nullable: false),
                        TeamId = c.Int(),
                        IsTeamLeader = c.Boolean(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.TeamId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        OrganisationId = c.Int(),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organisations", t => t.OrganisationId)
                .Index(t => t.OrganisationId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        OrganisationId = c.Int(),
                        TeamAssignedToId = c.Int(),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organisations", t => t.OrganisationId)
                .ForeignKey("dbo.Teams", t => t.TeamAssignedToId)
                .Index(t => t.OrganisationId)
                .Index(t => t.TeamAssignedToId);
            
            CreateTable(
                "dbo.RoleNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        NotificationAboutId = c.String(nullable: false, maxLength: 128),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.NotificationAboutId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.NotificationAboutId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserTo_Id, cascadeDelete: true)
                .Index(t => t.UserTo_Id);
            
            CreateTable(
                "dbo.TicketCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ProjectId = c.Int(),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.TicketLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(nullable: false),
                        TicketLogTypeId = c.Int(nullable: false),
                        Data = c.String(nullable: false),
                        TimeOfLog = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .ForeignKey("dbo.TicketLogTypes", t => t.TicketLogTypeId, cascadeDelete: true)
                .Index(t => t.TicketId)
                .Index(t => t.TicketLogTypeId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 250),
                        OpenedById = c.String(nullable: false, maxLength: 128),
                        TicketPriorityId = c.Int(nullable: false),
                        UserAssignedToId = c.String(maxLength: 128),
                        TeamAssignedToId = c.Int(),
                        OrganisationAssignedToId = c.Int(),
                        TicketStateId = c.Int(nullable: false),
                        ProjectId = c.Int(),
                        TicketCategoryId = c.Int(nullable: false),
                        Deadline = c.DateTime(),
                        LastMessage = c.DateTime(),
                        LastResponse = c.DateTime(),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OpenedById, cascadeDelete: true)
                .ForeignKey("dbo.Organisations", t => t.OrganisationAssignedToId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.Teams", t => t.TeamAssignedToId)
                .ForeignKey("dbo.TicketCategories", t => t.TicketCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.TicketPriorities", t => t.TicketPriorityId, cascadeDelete: true)
                .ForeignKey("dbo.TicketStates", t => t.TicketStateId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserAssignedToId)
                .Index(t => t.OpenedById)
                .Index(t => t.TicketPriorityId)
                .Index(t => t.UserAssignedToId)
                .Index(t => t.TeamAssignedToId)
                .Index(t => t.OrganisationAssignedToId)
                .Index(t => t.TicketStateId)
                .Index(t => t.ProjectId)
                .Index(t => t.TicketCategoryId);
            
            CreateTable(
                "dbo.TicketPriorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Colour = c.String(maxLength: 10),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Colour = c.String(maxLength: 10),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        NotificationAboutId = c.String(nullable: false, maxLength: 128),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.NotificationAboutId, cascadeDelete: true)
                .Index(t => t.NotificationAboutId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "NotificationAboutId", "dbo.Users");
            DropForeignKey("dbo.TicketLogs", "TicketLogTypeId", "dbo.TicketLogTypes");
            DropForeignKey("dbo.TicketLogs", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "UserAssignedToId", "dbo.Users");
            DropForeignKey("dbo.Tickets", "TicketStateId", "dbo.TicketStates");
            DropForeignKey("dbo.Tickets", "TicketPriorityId", "dbo.TicketPriorities");
            DropForeignKey("dbo.Tickets", "TicketCategoryId", "dbo.TicketCategories");
            DropForeignKey("dbo.Tickets", "TeamAssignedToId", "dbo.Teams");
            DropForeignKey("dbo.Tickets", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Tickets", "OrganisationAssignedToId", "dbo.Organisations");
            DropForeignKey("dbo.Tickets", "OpenedById", "dbo.Users");
            DropForeignKey("dbo.TicketCategories", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.TextMessages", "UserTo_Id", "dbo.Users");
            DropForeignKey("dbo.RoleNotifications", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleNotifications", "NotificationAboutId", "dbo.Users");
            DropForeignKey("dbo.Projects", "TeamAssignedToId", "dbo.Teams");
            DropForeignKey("dbo.Projects", "OrganisationId", "dbo.Organisations");
            DropForeignKey("dbo.Organisations", "DefaultContactId", "dbo.Users");
            DropForeignKey("dbo.Users", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Teams", "OrganisationId", "dbo.Organisations");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropIndex("dbo.UserNotifications", new[] { "NotificationAboutId" });
            DropIndex("dbo.Tickets", new[] { "TicketCategoryId" });
            DropIndex("dbo.Tickets", new[] { "ProjectId" });
            DropIndex("dbo.Tickets", new[] { "TicketStateId" });
            DropIndex("dbo.Tickets", new[] { "OrganisationAssignedToId" });
            DropIndex("dbo.Tickets", new[] { "TeamAssignedToId" });
            DropIndex("dbo.Tickets", new[] { "UserAssignedToId" });
            DropIndex("dbo.Tickets", new[] { "TicketPriorityId" });
            DropIndex("dbo.Tickets", new[] { "OpenedById" });
            DropIndex("dbo.TicketLogs", new[] { "TicketLogTypeId" });
            DropIndex("dbo.TicketLogs", new[] { "TicketId" });
            DropIndex("dbo.TicketCategories", new[] { "ProjectId" });
            DropIndex("dbo.TextMessages", new[] { "UserTo_Id" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.RoleNotifications", new[] { "NotificationAboutId" });
            DropIndex("dbo.RoleNotifications", new[] { "RoleId" });
            DropIndex("dbo.Projects", new[] { "TeamAssignedToId" });
            DropIndex("dbo.Projects", new[] { "OrganisationId" });
            DropIndex("dbo.Teams", new[] { "OrganisationId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Users", new[] { "TeamId" });
            DropIndex("dbo.Organisations", new[] { "DefaultContactId" });
            DropTable("dbo.UserNotifications");
            DropTable("dbo.TicketLogTypes");
            DropTable("dbo.TicketStates");
            DropTable("dbo.TicketPriorities");
            DropTable("dbo.Tickets");
            DropTable("dbo.TicketLogs");
            DropTable("dbo.TicketCategories");
            DropTable("dbo.TextMessages");
            DropTable("dbo.Roles");
            DropTable("dbo.RoleNotifications");
            DropTable("dbo.Projects");
            DropTable("dbo.Teams");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Organisations");
        }
    }
}
