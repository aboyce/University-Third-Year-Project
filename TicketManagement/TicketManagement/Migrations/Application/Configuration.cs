using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketManagement.Models.Entities;

namespace TicketManagement.Migrations.Application
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketManagement.Models.Context.ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Application";
        }

        protected override void Seed(TicketManagement.Models.Context.ApplicationContext context)
        {
            if (!new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)).RoleExists("Administrator"))
                context.Roles.AddOrUpdate(
                new IdentityRole("Approved"),
                new IdentityRole("Internal"),
                new IdentityRole("Social"),
                new IdentityRole("TextMessage"),
                new IdentityRole("Administrator")); 


            //UserExtra adminUser = new UserExtra { FirstName = "Admin", LastName = "Admin", IsArchived = false};
            //User admin = new User { UserName = "Admin", Email = "admin@email.com", UserExtra = adminUser, UserExtraId = adminUser.Id, PasswordHash = "AKZ158r4VoQTWkCp12qBlRydNbWc/n8wNs7ysjxXGr5ktfKN4j37RGAqcTd/T2ZGmg==" };

            //if (!context.Users.Contains(admin))
            //{
            //    context.UserExtras.AddOrUpdate(adminUser);
            //    context.Users.AddOrUpdate(admin);

            //    var userManager = new UserManager<User>(new UserStore<User>(context));
            //    userManager.AddToRole(admin.Id, "Administrator");
            //}

            if (!context.Organisations.Any(org => org.Name == "My Company"))
            {
                context.Organisations.AddOrUpdate(
                new Organisation { Name = "My Company", IsInternal = true, DefaultContact = null },
                new Organisation { Name = "Client", IsInternal = false, DefaultContact = null });
            }

            if (!context.Teams.Any(t => t.Name == "Support"))
            {
                context.Teams.AddOrUpdate(
                new Team { Name = "Support" },
                new Team { Name = "Management" });
            }

            if (!context.Projects.Any(p => p.Name == "First Application"))
            {
                context.Projects.AddOrUpdate(
                new Project { Name = "First Application" },
                new Project { Name = "Second Application" });
            }

            if (!context.TicketCategories.Any(tc => tc.Name == "Question"))
            {
                context.TicketCategories.AddOrUpdate(
                new TicketCategory { Name = "Question" },
                new TicketCategory { Name = "Bug" },
                new TicketCategory { Name = "Feature" });
            }

            if (!context.TicketLogTypes.Any(tlt => tlt.Name == "Message"))
            {
                context.TicketLogTypes.AddOrUpdate(
                new TicketLogType { Name = "Message" },
                new TicketLogType { Name = "Message" });
            }

            if (!context.TicketPriorities.Any(tc => tc.Name == "Low"))
            {
                context.TicketPriorities.AddOrUpdate(
                new TicketPriority { Name = "On Hold", Colour = "#0066FF" },
                new TicketPriority { Name = "Low", Colour = "#00CC00" },
                new TicketPriority { Name = "Medium", Colour = "#FF6600" },
                new TicketPriority { Name = "High", Colour = "#FF0000" },
                new TicketPriority { Name = "Emergency", Colour = "#800000" });
            }

            if (!context.TicketStates.Any(ts => ts.Name == "Pending Approval"))
            {
                context.TicketStates.AddOrUpdate(
                new TicketState { Name = "Pending Approval", Colour = "#009900" },
                new TicketState { Name = "Open", Colour = "#CCFFFF" },
                new TicketState { Name = "Awaiting Response", Colour = "#FFFFFF" },
                new TicketState { Name = "Closed", Colour = "#FF0000" });
            }
        }
    }
}
