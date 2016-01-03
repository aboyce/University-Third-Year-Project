using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;

namespace TicketManagement.Migrations.Application
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Application";
        }

        protected override void Seed(ApplicationContext context)
        {
            if (!new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)).RoleExists(MyRoles.Administrator))
                context.Roles.AddOrUpdate(
                new IdentityRole(MyRoles.Approved),
                new IdentityRole(MyRoles.Internal),
                new IdentityRole(MyRoles.Social),
                new IdentityRole(MyRoles.TextMessage),
                new IdentityRole(MyRoles.Administrator));

            if (!context.Users.Any(user => user.UserName == "admin"))
            {
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                User user = new User
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "Admin",
                    Email = "admin@email.com",
                    PhoneNumber = "00000000000",
                    IsArchived = false
                };

                userManager.Create(user, "admin!23");
                userManager.AddToRoles(user.Id, MyRoles.Approved, MyRoles.Internal, MyRoles.Administrator, MyRoles.Social, MyRoles.TextMessage);
            }

            if (!context.Organisations.Any(org => org.Name == "Your Company Name"))
            {
                Organisation org = new Organisation
                {
                    Name = "Your Company Name",
                    IsInternal = true,
                    DefaultContact = null
                };

                context.Organisations.AddOrUpdate(org);
                context.SaveChanges();

                Team supportTeam = new Team
                {
                    Name = "Support",
                    OrganisationId = org.Id,
                    Organisation = org
                };

                Team managementTeam = new Team
                {
                    Name = "Management",
                    OrganisationId = org.Id,
                    Organisation = org
                };

                context.Teams.AddOrUpdate(supportTeam);
                context.Teams.AddOrUpdate(managementTeam);
                context.SaveChanges();
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
