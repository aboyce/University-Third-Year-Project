using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;
using TicketManagement.Properties;

namespace TicketManagement.Helpers
{
    public class DataPopulationHelper
    {
        private const string demoPassword = "$56Def";
        private const string internalEmailEnd = "@yourcompanyname.com";
        private const string org1Name = "John's Complete Solutions";
        private const string org1Email = "@johnscompletesolutions.com";
        private const string org2Name = "Sally's Software";
        private const string org2Email = "@sallys-software.com";
        private const string org3Name = "Bob's Bytes";
        private const string org3Email = "@bobsbytes.com";

        public async Task<bool> PopulateDemoDataAsync(ApplicationContext db, UserManager<User> um)
        {
            #region Internal Users

            if (await db.Users.Where(u => u.UserName == "Steve_Brown").CountAsync() > 0)
                return false;

            User internalUser1 = new User
            {
                FirstName = "Steve",
                LastName = "Brown",
                UserName = "Steve_Brown", // Update the check above if this changes.
                Email = $"steve{internalEmailEnd}",
                PhoneNumber = "01234567890",
                IsArchived = false
            };

            User internalUser2 = new User
            {
                FirstName = "Shelly",
                LastName = "Green",
                UserName = "Shelly_Green",
                Email = $"shelly{internalEmailEnd}",
                PhoneNumber = "01234567890",
                IsArchived = false
            };

            User internalUser3 = new User
            {
                FirstName = "Guy",
                LastName = "Roberts",
                UserName = "Guy_Roberts",
                Email = $"guy{internalEmailEnd}",
                PhoneNumber = "01234567890",
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
                PhoneNumber = "01234567890",
                IsArchived = false
            };

            User org1User2 = new User
            {
                FirstName = "Barry",
                LastName = "Tompson",
                UserName = "Barry_Tompson",
                Email = $"barry{org1Email}",
                PhoneNumber = "01234567890",
                IsArchived = false
            };

            User org1User3 = new User
            {
                FirstName = "Susie",
                LastName = "Dry",
                UserName = "Susie_Dry",
                Email = $"susie{org1Email}",
                PhoneNumber = "01234567890",
                IsArchived = false
            };

            um.Create(org1User1, demoPassword);
            um.Create(org1User2, demoPassword);
            um.Create(org1User3, demoPassword);

            await um.AddToRoleAsync(org1User1.Id, "Approved");
            await um.AddToRoleAsync(org1User2.Id, "Approved");
            await um.AddToRoleAsync(org1User3.Id, "Approved");

            #endregion

            #region Organisation Users (Sally's Software)

            User org2User1 = new User
            {
                FirstName = "Sally",
                LastName = "Jones",
                UserName = "Sally_Jones",
                Email = $"sally{org2Email}",
                PhoneNumber = "01234567890",
                IsArchived = false
            };

            um.Create(org2User1, demoPassword);

            await um.AddToRoleAsync(org2User1.Id, "Approved");

            #endregion

            #region Organisation Users (Bobs's Bytes)

            User org3User1 = new User
            {
                FirstName = "Bob",
                LastName = "Angon",
                UserName = "Bob_Angon",
                Email = $"bob{org3Email}",
                PhoneNumber = "01234567890",
                IsArchived = false
            };

            User org3User2 = new User
            {
                FirstName = "Clair",
                LastName = "Angon",
                UserName = "Clair_Angon",
                Email = $"clair{org3Email}",
                PhoneNumber = "01234567890",
                IsArchived = false
            };

            um.Create(org3User1, demoPassword);
            um.Create(org3User2, demoPassword);

            await um.AddToRoleAsync(org3User1.Id, "Approved");
            await um.AddToRoleAsync(org3User2.Id, "Approved");

            #endregion

            await PopulateDataAsync(db);

            return true;
        }

        private async Task<bool> PopulateDataAsync(ApplicationContext db)
        {
            #region Internal Users

            User internalUser1 = await db.Users.Where(u => u.UserName == "Steve_Brown").Select(u => u).FirstOrDefaultAsync();

            User internalUser2 = await db.Users.Where(u => u.UserName == "Shelly_Green").Select(u => u).FirstOrDefaultAsync();

            User internalUser3 = await db.Users.Where(u => u.UserName == "Guy_Roberts").Select(u => u).FirstOrDefaultAsync();

            #endregion

            #region Organisation Users (John's Complete Solutions)

            User org1User1 = await db.Users.Where(u => u.UserName == "John_Smith").Select(u => u).FirstOrDefaultAsync();

            User org1User2 = await db.Users.Where(u => u.UserName == "Barry_Tompson").Select(u => u).FirstOrDefaultAsync();

            User org1User3 = await db.Users.Where(u => u.UserName == "Susie_Dry").Select(u => u).FirstOrDefaultAsync();

            #endregion

            #region Organisation Users (Sally's Software)

            User org2User1 = await db.Users.Where(u => u.UserName == "Sally_Jones").Select(u => u).FirstOrDefaultAsync();

            #endregion

            #region Organisation Users (Bobs's Bytes)

            User org3User1 = await db.Users.Where(u => u.UserName == "Bob_Angon").Select(u => u).FirstOrDefaultAsync();

            User org3User2 = await db.Users.Where(u => u.UserName == "Clair_Angon").Select(u => u).FirstOrDefaultAsync();

            #endregion

            #region Organisations

            Organisation org1 = new Organisation
            {
                Name = $"{org1Name}",
                IsInternal = false,
                DefaultContact = await db.Users.FirstOrDefaultAsync(u => u.Id == org1User1.Id)
            };

            Organisation org2 = new Organisation
            {
                Name = $"{org2Name}",
                IsInternal = false,
                DefaultContact = await db.Users.FirstOrDefaultAsync(u => u.Id == org2User1.Id)
            };

            Organisation org3 = new Organisation
            {
                Name = $"{org3Name}",
                IsInternal = false,
                DefaultContact = await db.Users.FirstOrDefaultAsync(u => u.Id == org3User1.Id)
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
                Name = $"{org1Name} - Estimates for Work",
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
                Deadline = DateTime.Now.AddDays(5),
                Created = DateTime.Now.AddDays(-2)
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
                Deadline = DateTime.Now.AddDays(5),
                Created = DateTime.Now.AddDays(-1)
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
                Deadline = DateTime.Now.AddDays(7),
                Created = DateTime.Now.AddDays(-1)
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
                Deadline = DateTime.Now.AddDays(20),
                Created = DateTime.Now.AddDays(-5)
            };

            db.Tickets.AddOrUpdate(org1Ticket1);
            db.Tickets.AddOrUpdate(org1Ticket2);
            db.Tickets.AddOrUpdate(org2Ticket1);
            db.Tickets.AddOrUpdate(org3Ticket1);

            await db.SaveChangesAsync();

            #endregion

            #region Files

            byte[] file1Content = new byte[Resources.DataPopulation_File1_Content.Length * sizeof(char)];
            Buffer.BlockCopy(Resources.DataPopulation_File1_Content.ToCharArray(), 0, file1Content, 0, file1Content.Length);

            File file1 = new File
            {
                FileName = "PDF of Related Data",
                FileType = FileType.PDF,
                ContentType = "application/pdf",
                Content = file1Content
            };

            #endregion

            #region Ticket Logs

            TicketLog ticket1Log1 = new TicketLog
            {
                TicketId = org1Ticket1.Id,
                Ticket = org1Ticket1,
                SubmittedByUserId = org1User2.Id,
                SubmittedByUser = org1User2,
                TicketLogType = TicketLogType.MessageFromExternalUser,
                Message = "Hello, would you be able to look at this for us please. Thanks",
                IsInternal = false,
                TimeOfLog = DateTime.Now.AddHours(-3)
            };

            TicketLog ticket1Log2 = new TicketLog
            {
                TicketId = org1Ticket1.Id,
                Ticket = org1Ticket1,
                SubmittedByUserId = internalUser2.Id,
                SubmittedByUser = internalUser2,
                TicketLogType = TicketLogType.MessageFromInternalUser,
                Message = "Hello, yes we will look into this and get back to you as soon as possible. Thanks",
                IsInternal = false,
                TimeOfLog = DateTime.Now.AddHours(-2)
            };

            TicketLog ticket1Log3 = new TicketLog
            {
                TicketId = org1Ticket1.Id,
                Ticket = org1Ticket1,
                SubmittedByUserId = internalUser2.Id,
                SubmittedByUser = internalUser2,
                TicketLogType = TicketLogType.MessageFromInternalUser,
                Message = "Guy, can you look into this?",
                IsInternal = true,
                TimeOfLog = DateTime.Now.AddHours(-1)
            };

            TicketLog ticket2Log1 = new TicketLog
            {
                TicketId = org1Ticket2.Id,
                Ticket = org1Ticket2,
                SubmittedByUserId = org1User1.Id,
                SubmittedByUser = org1User1,
                TicketLogType = TicketLogType.MessageFromExternalUser,
                Message = "Hello, sorry to bug you, but we could do with an update on this matter. Thanks",
                IsInternal = false,
                TimeOfLog = DateTime.Now.AddHours(-8)
            };

            TicketLog ticket2Log2 = new TicketLog
            {
                TicketId = org1Ticket2.Id,
                Ticket = org1Ticket2,
                SubmittedByUserId = internalUser3.Id,
                SubmittedByUser = internalUser3,
                TicketLogType = TicketLogType.MessageFromInternalUser,
                Message = "No problem, we have emailed the new estimate, we will now close this ticket. Thanks",
                IsInternal = false,
                TimeOfLog = DateTime.Now.AddHours(-7)
            };

            TicketLog ticket3Log1 = new TicketLog
            {
                TicketId = org2Ticket1.Id,
                Ticket = org2Ticket1,
                SubmittedByUserId = org2User1.Id,
                SubmittedByUser = org2User1,
                TicketLogType = TicketLogType.MessageFromExternalUser,
                Message = "We have had our team look at this overnight but we cannot get it to turn on, would you be able to assist please.",
                IsInternal = false,
                TimeOfLog = DateTime.Now.AddHours(-6)
            };

            TicketLog ticket3Log2 = new TicketLog
            {
                TicketId = org2Ticket1.Id,
                Ticket = org2Ticket1,
                SubmittedByUserId = internalUser1.Id,
                SubmittedByUser = internalUser1,
                TicketLogType = TicketLogType.MessageFromInternalUser,
                Message = "We will send a team down, please expect a call within 24 hours.",
                IsInternal = false,
                TimeOfLog = DateTime.Now.AddHours(-5)
            };

            TicketLog ticket4Log1 = new TicketLog
            {
                TicketId = org3Ticket1.Id,
                Ticket = org3Ticket1,
                SubmittedByUserId = org3User2.Id,
                SubmittedByUser = org3User2,
                TicketLogType = TicketLogType.MessageFromExternalUser,
                Message = "Unfortunately we have made a mistake and require the workload to be reinvestigated.",
                IsInternal = false,
                TimeOfLog = DateTime.Now.AddHours(-5)
            };

            TicketLog ticket4Log2 = new TicketLog
            {
                TicketId = org3Ticket1.Id,
                Ticket = org3Ticket1,
                SubmittedByUserId = org3User2.Id,
                SubmittedByUser = org3User2,
                TicketLogType = TicketLogType.MessageFromExternalUser,
                FileId = file1.Id,
                File = file1,
                Message = "Here is the copy of the document mentioned on the phone, this should help you out.",
                IsInternal = false,
                TimeOfLog = DateTime.Now.AddHours(-5)
            };


            db.TicketLogs.Add(ticket1Log1); 
            db.TicketLogs.Add(ticket1Log2);
            db.TicketLogs.Add(ticket1Log3);
            db.TicketLogs.Add(ticket2Log1);
            db.TicketLogs.Add(ticket2Log2);
            db.TicketLogs.Add(ticket3Log1);
            db.TicketLogs.Add(ticket3Log2);
            db.TicketLogs.Add(ticket4Log1);
            db.TicketLogs.Add(ticket4Log2);

            await db.SaveChangesAsync();

            #endregion

            return true;
        }
    }
}
