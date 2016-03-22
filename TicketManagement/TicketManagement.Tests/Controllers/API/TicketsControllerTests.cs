using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagement.Controllers.API;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Tests.Controllers.API
{
    [TestClass]
    public class TicketsControllerTests : TestBase
    {
        [TestMethod]
        public async Task Test_TicketsController_GetAllTicketsForUser()
        {
            // Setup.
            SeedDatabase();
            TicketsController controllerUnderTest = new TicketsController(Database);




        }


        protected override async void SeedDatabase()
        {
            if (!new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Database)).RoleExists(MyRoles.Administrator))
                Database.Roles.AddOrUpdate(
                new IdentityRole(MyRoles.Approved),
                new IdentityRole(MyRoles.Internal),
                new IdentityRole(MyRoles.Social),
                new IdentityRole(MyRoles.TextMessage),
                new IdentityRole(MyRoles.Administrator));

            if (!Database.Users.Any(user => user.UserName == "admin"))
            {
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(Database));

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

            if (!Database.Organisations.Any(org => org.Name == "Your Company Name"))
            {
                Organisation org = new Organisation
                {
                    Name = "Your Company Name",
                    IsInternal = true,
                    DefaultContact = null
                };

                Database.Organisations.AddOrUpdate(org);
                Database.SaveChanges();

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

                Database.Teams.AddOrUpdate(supportTeam);
                Database.Teams.AddOrUpdate(managementTeam);
                Database.SaveChanges();
            }

            if (!Database.TicketCategories.Any(tc => tc.Name == "Question"))
            {
                Database.TicketCategories.AddOrUpdate(
                new TicketCategory { Name = "Question" },
                new TicketCategory { Name = "Bug" },
                new TicketCategory { Name = "Feature" });
            }

            if (!Database.TicketPriorities.Any(tc => tc.Name == "Low"))
            {
                Database.TicketPriorities.AddOrUpdate(
                new TicketPriority { Name = "On Hold", Colour = "#0066FF" },
                new TicketPriority { Name = "Low", Colour = "#00CC00" },
                new TicketPriority { Name = "Medium", Colour = "#FF6600" },
                new TicketPriority { Name = "High", Colour = "#FF0000" },
                new TicketPriority { Name = "Emergency", Colour = "#800000" });
            }

            if (!Database.TicketStates.Any(ts => ts.Name == "Pending Approval"))
            {
                Database.TicketStates.AddOrUpdate(
                new TicketState { Name = "Pending Approval", Colour = "#009900" },
                new TicketState { Name = "Open", Colour = "#CCFFFF" },
                new TicketState { Name = "Awaiting Response", Colour = "#FFFFFF" },
                new TicketState { Name = "Closed", Colour = "#FF0000" });
            }

            if(!await new DataPopulationHelper().PopulateDemoDataAsync(Database, new UserManager<User>(new UserStore<User>(Database))))
                throw new Exception("TicketsControllerTest:SeedDatabase: Could not populate the database.");
        }
    }
}
