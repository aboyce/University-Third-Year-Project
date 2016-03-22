using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagement.Controllers.API;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Tests.Controllers.API
{
    [TestClass]
    public class TicketsControllerTests : TestBase
    {
        //[TestMethod]
        public async Task Test_TicketsController_GetAllTicketsForUser()
        {
            // Setup.
            SeedDatabase();
            TicketsController controllerUnderTest = new TicketsController(Database);




        }


        protected override void SeedDatabase()
        {

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

        }
    }
}
