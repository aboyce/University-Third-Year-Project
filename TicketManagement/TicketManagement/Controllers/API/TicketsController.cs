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

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<JsonResult> GetAllTicketsForUser(string username, string usertoken)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(usertoken)) return null;

            // Try to get the specific user with the Username and UserToken
            string userId = await db.Users.Where(u => u.UserName == username && u.UserToken == usertoken).Select(u => u.Id).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(userId)) return null;

            // Build a query for all the tickets and linked information. (Not an actual DB request)
            var tickets = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);

            // Get the Id for the Role, Internal.
            string internalRoleId = await db.Roles.Where(r => r.Name == MyRoles.Internal).Select(r => r.Id).FirstOrDefaultAsync();
            // Get the Users that are internal.
            List<User> internalUsers = await db.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(internalRoleId)).ToListAsync();

            // If the user is not internal than they should only be able to see thier tickets.
            if (internalUsers.Find(u => u.Id == userId) == null)
                tickets = tickets.Where(t => t.OpenedById == userId);

            JsonResult temp = new JsonResult
            {
                ContentType = "Tickets",
                Data = tickets.Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList()
            };
            return temp;

            //return Json(tickets.Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList()); // TODO: Test that this performs as expected...
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public JsonResult<List<ApiTicketViewModel>> GetTicketsAssignedTo(string userId)
        {
           return Json(db.Tickets.Where(t => t.UserAssignedToId == userId).ToList().Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList());
        }



    }
}
