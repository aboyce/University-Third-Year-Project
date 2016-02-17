using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers.API
{
    [AllowAnonymous]
    public class TicketsController : BaseApiController
    {
        private ApplicationContext db = new ApplicationContext();

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<JsonResult<List<ApiTicketViewModel>>> GetAllTicketsForUser(string username, string usertoken)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(usertoken)) return null;

            string userId = await db.Users.Where(u => u.UserName == username && u.UserToken == usertoken).Select(u => u.Id).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(userId)) return null;

            var tickets = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);

            string internalRoleId = await db.Roles.Where(r => r.Name == MyRoles.Internal).Select(r => r.Id).FirstOrDefaultAsync();

            var users = await db.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(internalRoleId)).ToListAsync();

            if(users.Find(u => u.Id == userId) == null) // If the user is not internal than they should only be able to see thier tickets.
                tickets = tickets.Where(t => t.OpenedById == userId);

            return Json(tickets.Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList()); // TODO: Test that this performs as expected...
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public JsonResult<List<ApiTicketViewModel>> GetTicketsAssignedTo(string userId)
        {
            return Json(db.Tickets.Where(t => t.UserAssignedToId == userId).ToList().Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList());
        }



    }
}
