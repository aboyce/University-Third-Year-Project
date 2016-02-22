using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers.API
{
    [AllowAnonymous]
    public class TicketsController : BaseApiController
    {
        private ApplicationContext db = new ApplicationContext();

        private async Task<bool> IsUserInternal(string userId)
        {
            // Get the Id for the Role, Internal.
            string internalRoleId = await db.Roles.Where(r => r.Name == MyRoles.Internal).Select(r => r.Id).FirstOrDefaultAsync();
            // Get the Users that are internal.
            List<User> internalUsers = await db.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(internalRoleId)).ToListAsync();
            // If the user is not in the list of internal users, then return false.
            return internalUsers.Find(u => u.Id == userId) != null;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<JsonResult> GetAllTicketsForUser(string username, string usertoken)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(usertoken)) return null;

            // Try to get the specific user with the Username and UserToken
            string userId = await db.Users.Where(u => u.UserName == username && u.UserToken == usertoken).Select(u => u.Id).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(userId)) return null;

            // Build a query for all the tickets and linked information. (Not an actual DB request)
            var tickets = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);

            if(!await IsUserInternal(userId))
                tickets = tickets.Where(t => t.OpenedById == userId);

            return new JsonResult
            {
                ContentType = "Tickets",
                Data = tickets.Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList()
            };
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<JsonResult> GetTicketForUser(string ticketid, string username, string usertoken)
        {
            if (string.IsNullOrEmpty(ticketid) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(usertoken)) return null;

            // See if the Ticket Id is a valid value, and cast to an int.
            int id;
            if (!int.TryParse(ticketid, out id)) return null;

            // Try to get the specific user with the Username and UserToken
            string userId = await db.Users.Where(u => u.UserName == username && u.UserToken == usertoken).Select(u => u.Id).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(userId)) return null;

            // Build a query for the ticket and linked information. (Not an actual DB request)
            var ticketQuery = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);

            // Try to get the ticket
            Ticket ticket = await ticketQuery.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null) return null;

            // If the user is internal, then we can give them the ticket information.
            if (await IsUserInternal(userId))
            {
                return new JsonResult
                {
                    ContentType = "Ticket",
                    Data = Helpers.ApiHelper.GetApiTicketViewModel(ticket)
                };
            }

            // If the user is not internal, we have to check that they opened the ticket, aka it is one of theirs before giving them the information.
            else
            {
                if (ticket.OpenedById != userId)
                    return null;

                return new JsonResult
                {
                    ContentType = "Ticket",
                    Data = Helpers.ApiHelper.GetApiTicketViewModel(ticket)
                };
            }
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<JsonResult> GetTicketLogsForUser(string ticketid, string username, string usertoken)
        {
            if (string.IsNullOrEmpty(ticketid) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(usertoken)) return null;

            // See if the Ticket Id is a valid value, and cast to an int.
            int id;
            if (!int.TryParse(ticketid, out id)) return null;

            // Build a query for the ticket and linked information. (Not an actual DB request)
            var ticketQuery = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);

            // Try to get the ticket
            Ticket ticket = await ticketQuery.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null) return null;

            // Try to get the specific user with the Username and UserToken
            string userId = await db.Users.Where(u => u.UserName == username && u.UserToken == usertoken).Select(u => u.Id).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(userId)) return null;

            // Get a list of the Ticket Logs that are related to the ticket
            var ticketLogs = db.TicketLogs.Where(tl => tl.TicketId == id).Include(tl => tl.Ticket).Include(tl => tl.SubmittedByUser);

            // If the user is not internal...
            if (! await IsUserInternal(userId))
            { 
                // ... they must have opened it to be able to access the information, else they don' have permission to view the information.
                if (ticket.OpenedById != userId)
                    return null;

                // ... exclude any internal messages regarding the ticket, as they also don't have permission to view the information
                ticketLogs = ticketLogs.Where(tl => tl.IsInternal == false);
            }

            return new JsonResult
            {
                ContentType = "TicketLogs",
                Data = ticketLogs.Select(Helpers.ApiHelper.GetApiTicketLogViewModel).ToList()
            };
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public JsonResult<List<ApiTicketViewModel>> GetTicketsAssignedTo(string userId)
        {
           return Json(db.Tickets.Where(t => t.UserAssignedToId == userId).ToList().Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList());
        }



    }
}
