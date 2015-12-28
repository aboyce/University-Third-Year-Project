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
            string demoPassword = "$56Def";
            string internalEmailEnd = "@ticketsystemteam.com";

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
                Name = $"{org1Name} - Management",
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

            Project org1Pro1 = new Project
            {
                Name = $"{org1Name} - New Website",
                OrganisationId = org1.Id,
                Organisation = org1,
                TeamAssignedToId = org1Team1.Id,
                TeamAssignedTo = org1Team1
            };

            Project org2Pro1 = new Project
            {
                Name = $"{org2Name} - Update Server",
                OrganisationId = org2.Id,
                Organisation = org2,
                TeamAssignedToId = org1Team1.Id,
                TeamAssignedTo = org1Team1
            };

            #endregion



            return true;
        }
    }
}
