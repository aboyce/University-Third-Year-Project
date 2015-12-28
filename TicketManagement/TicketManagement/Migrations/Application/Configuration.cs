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

            if (!context.Users.Any(user => user.UserName == "admin"))
            {
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                User user = new User { FirstName = "Admin", LastName = "Admin", UserName = "Admin", Email = "admin@email.com", IsArchived = false };

                userManager.Create(user, "admin!23");
                userManager.AddToRoles(user.Id, "Approved", "Internal", "Administrator", "Social", "TextMessage");
            }

            if (!context.Organisations.Any(org => org.Name == "Ticket System"))
            {
                context.Organisations.AddOrUpdate(new Organisation
                {
                    Name = "Ticket System",
                    IsInternal = true,
                    DefaultContact = null
                });


            }

            if (!context.Teams.Any(t => t.Name == "Support"))
            {
                context.Teams.AddOrUpdate(
                new Team { Name = "Support" },
                new Team { Name = "Management" });
            }

            if (!context.TicketCategories.Any(tc => tc.Name == "Question"))
            {
                context.TicketCategories.AddOrUpdate(
                new TicketCategory { Name = "Question" },
                new TicketCategory { Name = "Bug" },
                new TicketCategory { Name = "Feature" });
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
