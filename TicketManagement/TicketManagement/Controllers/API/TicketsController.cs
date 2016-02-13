using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using Newtonsoft.Json;
using TicketManagement.Models.Context;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers.API
{
    [AllowAnonymous]
    public class TicketsController : BaseApiController
    {
        private ApplicationContext db = new ApplicationContext();

        public JsonResult<List<ApiTicketViewModel>> GetAllTickets()
        {
            var tickets = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);
            return Json(tickets.Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList());
        }

        public JsonResult<List<ApiTicketViewModel>> GetTicketsAssignedTo(string userId)
        {
            return Json(db.Tickets.Where(t => t.UserAssignedToId == userId).ToList().Select(Helpers.ApiHelper.GetApiTicketViewModel).ToList());
        }



    }
}
