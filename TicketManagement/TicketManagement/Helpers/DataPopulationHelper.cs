using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;

namespace TicketManagement.Helpers
{
    public class DataPopulationHelper
    {
        public async Task<bool> PopulateDemoDataAsync(ApplicationContext db, UserManager<User> um)
        {
            if (!await PopulateEntitiesAsync(db, um)) return false;

            return true;
        }

        private async Task<bool> PopulateEntitiesAsync(ApplicationContext db, UserManager<User> um)
        {
            const string demoPassword = "$56Def";
            const string internalEmailEnd = "@yourcompanyname.com";
            const string org1Name = "John's Complete Solutions";
            const string org1Email = "@johnscompletesolutions.com";
            const string org2Name = "Sally's Software";
            const string org2Email = "@sallys-software.com";
            const string org3Name = "Bob's Bytes";
            const string org3Email = "@bobsbytes.com";

            #region Internal Users

            User internalUser1 = new User
            {
                FirstName = "Steve",
                LastName = "Brown",
                UserName = "Steve_Brown",
                Email = $"steve{internalEmailEnd}",
                IsArchived = false
            };

            User internalUser2 = new User
            {
                FirstName = "Shelly",
                LastName = "Green",
                UserName = "Shelly_Green",
                Email = $"shelly{internalEmailEnd}",
                IsArchived = false
            };

            User internalUser3 = new User
            {
                FirstName = "Guy",
                LastName = "Roberts",
                UserName = "Guy_Roberts",
                Email = $"guy{internalEmailEnd}",
                IsArchived = false
            };

            um.Create(internalUser1, demoPassword);
            um.Create(internalUser2, demoPassword);
            um.Create(internalUser3, demoPassword);

            await um.AddToRolesAsync(internalUser1.Id, "Approved", "Internal");
            await um.AddToRolesAsync(internalUser2.Id, "Approved", "Internal");
            await um.AddToRolesAsync(internalUser3.Id, "Approved", "Internal");

            #endregion

            #region Organisation Users (John's Complete Solutions)

            User org1User1 = new User
            {
                FirstName = "John",
                LastName = "Smith",
                UserName = "John_Smith",
                Email = $"john{org1Email}",
                IsArchived = false
            };

            User org1User2 = new User
            {
                FirstName = "Barry",
                LastName = "Tompson",
                UserName = "Barry_Tompson",
                Email = $"barry{org1Email}",
                IsArchived = false
            };

            User org1User3 = new User
            {
                FirstName = "Susie",
                LastName = "Dry",
                UserName = "Susie_Dry",
                Email = $"susie{org1Email}",
                IsArchived = false
            };

            um.Create(org1User1, demoPassword);
            um.Create(org1User2, demoPassword);
            um.Create(org1User3, demoPassword);

            #endregion

            #region Organisation Users (Sally's Software)

            User org2User1 = new User
            {
                FirstName = "Sally",
                LastName = "Jones",
                UserName = "Sally_Jones",
                Email = $"sally{org2Email}",
                IsArchived = false
            };

            um.Create(org2User1, demoPassword);

            #endregion

            #region Organisation Users (Bobs's Bytes)

            User org3User1 = new User
            {
                FirstName = "Bob",
                LastName = "Angon",
                UserName = "Bob_Angon",
                Email = $"bob{org3Email}",
                IsArchived = false
            };

            User org3User2 = new User
            {
                FirstName = "Clair",
                LastName = "Angon",
                UserName = "Clair_Angon",
                Email = $"clair{org3Email}",
                IsArchived = false
            };

            um.Create(org3User1, demoPassword);
            um.Create(org3User2, demoPassword);

            #endregion

            #region Organisations

            Organisation org1 = new Organisation
            {
                Name = org1Name,
                IsInternal = false,
                DefaultContact = await db.Users.FirstOrDefaultAsync(u => u.Email == $"john{org1Email}")
            };

            Organisation org2 = new Organisation
            {
                Name = org2Name,
                IsInternal = false,
                DefaultContact = await db.Users.FirstOrDefaultAsync(u => u.Email == $"sally{org2Email}")
            };

            Organisation org3 = new Organisation
            {
                Name = org3Name,
                IsInternal = false,
                DefaultContact = await db.Users.FirstOrDefaultAsync(u => u.Email == $"bob{org3Email}")
            };

            db.Organisations.AddOrUpdate(org1);
            db.Organisations.AddOrUpdate(org2);
            db.Organisations.AddOrUpdate(org3);
            await db.SaveChangesAsync();

            #endregion

            #region Teams

            Team org1Team1 = new Team
            {
                Name = $"{org1Name} - Support",
                OrganisationId = org1.Id,
                Organisation = org1
            };

            Team org1Team2 = new Team
            {
                Name = $"{org1Name} - Accouting",
                OrganisationId = org1.Id,
                Organisation = org1
            };

            Team org2Team1 = new Team
            {
                Name = $"{org2Name} - Support",
                OrganisationId = org2.Id,
                Organisation = org2
            };

            Team org3Team1 = new Team
            {
                Name = $"{org3Name} - Support",
                OrganisationId = org3.Id,
                Organisation = org3
            };

            db.Teams.AddOrUpdate(org1Team1);
            db.Teams.AddOrUpdate(org1Team2);
            db.Teams.AddOrUpdate(org2Team1);
            db.Teams.AddOrUpdate(org3Team1);
            await db.SaveChangesAsync();

            #endregion

            #region Projects

            Organisation internalOrganisation = await db.Organisations.Where(o => o.IsInternal).Select(o => o).FirstOrDefaultAsync();
            Team internalSupportTeam = await db.Teams.Where(t => t.OrganisationId == internalOrganisation.Id && t.Name == "Support").Select(t => t).FirstOrDefaultAsync();
            Team internalManagementTeam = await db.Teams.Where(t => t.OrganisationId == internalOrganisation.Id && t.Name == "Management").Select(t => t).FirstOrDefaultAsync();

            Project org1Pro1 = new Project
            {
                Name = $"{org1Name} - New Website",
                OrganisationId = org1.Id,
                Organisation = org1,
                TeamAssignedToId = internalSupportTeam.Id,
                TeamAssignedTo = internalSupportTeam
            };

            Project org1Pro2 = new Project
            {
                Name = $"{org1Name} - Estimates for Work on Location",
                OrganisationId = org1.Id,
                Organisation = org1,
                TeamAssignedToId = internalManagementTeam.Id,
                TeamAssignedTo = internalManagementTeam
            };

            Project org2Pro1 = new Project
            {
                Name = $"{org2Name} - Update Server",
                OrganisationId = org2.Id,
                Organisation = org2,
                TeamAssignedToId = internalSupportTeam.Id,
                TeamAssignedTo = internalSupportTeam
            };

            Project org2Pro2 = new Project
            {
                Name = $"{org2Name} - Fix New Release",
                OrganisationId = org2.Id,
                Organisation = org2,
                TeamAssignedToId = internalSupportTeam.Id,
                TeamAssignedTo = internalSupportTeam
            };

            Project org2Pro3 = new Project
            {
                Name = $"{org2Name} - Data Migration",
                OrganisationId = org2.Id,
                Organisation = org2,
                TeamAssignedToId = internalSupportTeam.Id,
                TeamAssignedTo = internalSupportTeam
            };

            Project org3Pro1 = new Project
            {
                Name = $"{org3Name} - Price Up New Designs",
                OrganisationId = org3.Id,
                Organisation = org3,
                TeamAssignedToId = internalManagementTeam.Id,
                TeamAssignedTo = internalManagementTeam
            };

            db.Projects.AddOrUpdate(org1Pro1);
            db.Projects.AddOrUpdate(org1Pro2);
            db.Projects.AddOrUpdate(org2Pro1);
            db.Projects.AddOrUpdate(org2Pro2);
            db.Projects.AddOrUpdate(org2Pro3);
            db.Projects.AddOrUpdate(org3Pro1);
            await db.SaveChangesAsync();

            #endregion

            #region Tickets

            Ticket org1Ticket1 = new Ticket
            {
                Title = "The Edit page is not displaying correctly",
                Description = "The latest beta release has a problem with the user edit page",
                OpenedById = org1User1.Id,
                OpenedBy = org1User1,
                TicketPriorityId = await db.TicketPriorities.Where(tp => tp.Name == "Medium").Select(tp => tp.Id).FirstOrDefaultAsync(),
                TicketPriority = await db.TicketPriorities.Where(tp => tp.Name == "Medium").Select(tp => tp).FirstOrDefaultAsync(),
                TeamAssignedToId = internalSupportTeam.Id,
                TeamAssignedTo = internalSupportTeam,
                OrganisationAssignedToId = org1.Id,
                OrganisationAssignedTo = org1,
                TicketStateId = await db.TicketStates.Where(ts => ts.Name == "Open").Select(ts => ts.Id).FirstOrDefaultAsync(),
                TicketState = await db.TicketStates.Where(ts => ts.Name == "Open").Select(ts => ts).FirstOrDefaultAsync(),
                ProjectId = org1Pro1.Id,
                Project = org1Pro1,
                TicketCategoryId = await db.TicketCategories.Where(tc => tc.Name == "Bug").Select(tc => tc.Id).FirstOrDefaultAsync(),
                TicketCategory = await db.TicketCategories.Where(tc => tc.Name == "Bug").Select(tc => tc).FirstOrDefaultAsync(),
                Deadline = DateTime.Now.AddDays(5)
            };

            Ticket org1Ticket2 = new Ticket
            {
                Title = "Chasing up Estimates",
                Description = "Looking for an update on the estimates we discussed",
                OpenedById = org1User2.Id,
                OpenedBy = org1User2,
                TicketPriorityId = await db.TicketPriorities.Where(tp => tp.Name == "On Hold").Select(tp => tp.Id).FirstOrDefaultAsync(),
                TicketPriority = await db.TicketPriorities.Where(tp => tp.Name == "On Hold").Select(tp => tp).FirstOrDefaultAsync(),
                TeamAssignedToId = internalSupportTeam.Id,
                TeamAssignedTo = internalSupportTeam,
                OrganisationAssignedToId = org1.Id,
                OrganisationAssignedTo = org1,
                TicketStateId = await db.TicketStates.Where(ts => ts.Name == "Closed").Select(ts => ts.Id).FirstOrDefaultAsync(),
                TicketState = await db.TicketStates.Where(ts => ts.Name == "Closed").Select(ts => ts).FirstOrDefaultAsync(),
                ProjectId = org1Pro2.Id,
                Project = org1Pro2,
                TicketCategoryId = await db.TicketCategories.Where(tc => tc.Name == "Question").Select(tc => tc.Id).FirstOrDefaultAsync(),
                TicketCategory = await db.TicketCategories.Where(tc => tc.Name == "Question").Select(tc => tc).FirstOrDefaultAsync(),
                Deadline = DateTime.Now.AddDays(5)
            };

            Ticket org2Ticket1 = new Ticket
            {
                Title = "Problem turning the Server on",
                Description = "The server beeps when turned on",
                OpenedById = org2User1.Id,
                OpenedBy = org2User1,
                TicketPriorityId = await db.TicketPriorities.Where(tp => tp.Name == "Medium").Select(tp => tp.Id).FirstOrDefaultAsync(),
                TicketPriority = await db.TicketPriorities.Where(tp => tp.Name == "Medium").Select(tp => tp).FirstOrDefaultAsync(),
                TeamAssignedToId = internalSupportTeam.Id,
                TeamAssignedTo = internalSupportTeam,
                OrganisationAssignedToId = org2.Id,
                OrganisationAssignedTo = org2,
                TicketStateId = await db.TicketStates.Where(ts => ts.Name == "Open").Select(ts => ts.Id).FirstOrDefaultAsync(),
                TicketState = await db.TicketStates.Where(ts => ts.Name == "Open").Select(ts => ts).FirstOrDefaultAsync(),
                ProjectId = org2Pro1.Id,
                Project = org2Pro1,
                TicketCategoryId = await db.TicketCategories.Where(tc => tc.Name == "Question").Select(tc => tc.Id).FirstOrDefaultAsync(),
                TicketCategory = await db.TicketCategories.Where(tc => tc.Name == "Question").Select(tc => tc).FirstOrDefaultAsync(),
                Deadline = DateTime.Now.AddDays(7)
            };

            Ticket org3Ticket1 = new Ticket
            {
                Title = "Adding work to the initial specification",
                Description = "We would like to add on some more work for the quote",
                OpenedById = org3User2.Id,
                OpenedBy = org3User2,
                TicketPriorityId = await db.TicketPriorities.Where(tp => tp.Name == "Low").Select(tp => tp.Id).FirstOrDefaultAsync(),
                TicketPriority = await db.TicketPriorities.Where(tp => tp.Name == "Low").Select(tp => tp).FirstOrDefaultAsync(),
                TeamAssignedToId = internalManagementTeam.Id,
                TeamAssignedTo = internalManagementTeam,
                OrganisationAssignedToId = org3.Id,
                OrganisationAssignedTo = org3,
                TicketStateId = await db.TicketStates.Where(ts => ts.Name == "Awaiting Response").Select(ts => ts.Id).FirstOrDefaultAsync(),
                TicketState = await db.TicketStates.Where(ts => ts.Name == "Awaiting Response").Select(ts => ts).FirstOrDefaultAsync(),
                ProjectId = org3Pro1.Id,
                Project = org3Pro1,
                TicketCategoryId = await db.TicketCategories.Where(tc => tc.Name == "Feature").Select(tc => tc.Id).FirstOrDefaultAsync(),
                TicketCategory = await db.TicketCategories.Where(tc => tc.Name == "Feature").Select(tc => tc).FirstOrDefaultAsync(),
                Deadline = DateTime.Now.AddDays(20)
            };

            db.Tickets.AddOrUpdate(org1Ticket1);
            db.Tickets.AddOrUpdate(org1Ticket2);
            db.Tickets.AddOrUpdate(org2Ticket1);
            db.Tickets.AddOrUpdate(org3Ticket1);

            await db.SaveChangesAsync();

            #endregion

            #region Ticket Logs

            TicketLog ticket1Log1 = new TicketLog
            {
                TicketId = org1Ticket1.Id,
                Ticket = org1Ticket1,
                SubmittedByUserId = org2User1.Id,
                SubmittedByUser = org2User1,
                TicketLogType = TicketLogType.Message,
                Message = "Hello, would you be able to look at this for us please. Thanks",
                IsInternal = false,
                TimeOfLog = DateTime.Now
            };

            TicketLog ticket1Log2 = new TicketLog
            {
                TicketId = org1Ticket1.Id,
                Ticket = org1Ticket1,
                SubmittedByUserId = internalUser2.Id,
                SubmittedByUser = internalUser2,
                TicketLogType = TicketLogType.Message,
                Message = "Hello, yes we will look into this and get back to you as soon as possible. Thanks",
                IsInternal = false,
                TimeOfLog = DateTime.Now
            };

            db.TicketLogs.Add(ticket1Log1);
            db.TicketLogs.Add(ticket1Log2);

            await db.SaveChangesAsync();

            #endregion

            return true;
        }
    }
}
