using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Controllers.API
{
    public class BaseApiController : ApiController
    {
        protected ApplicationContext db;

        public BaseApiController() { db = new ApplicationContext(); } // TODO: Fix this, its getting called from the child classes...
        public BaseApiController(ApplicationContext context) { db = context; }

        private readonly string _response = $"Ticket System API v{Assembly.GetExecutingAssembly().GetName().Version}";

        private readonly string _availableMethods = " : Available API Calls :" +
                   "[Tickets/GetAllTicketsForUser(string username, string usertoken) => Json list of tickets.]" +
                   "[Tickets/GetTicketForUser(string ticketid, string username, string usertoken) => Json object (ticket).]" +
                   "[Tickets/GetTicketLogsForUser(string ticketid, string username, string usertoken) => Json list of tickets logs.]" +
                   "[Tickets/AddInternalReplyToTicket(string ticketid, string username, string usertoken, string message) => True or False.]" +
                   "[Tickets/AddExternalReplyToTicket(string ticketid, string username, string usertoken, string message) => True or False.]" +
                   "[User/GetNewUserToken(string username) => The user token and if the user is internal.]" +
                   "[User/CheckUserToken(string username, string usertoken) => true/false if the combination is valid.]" +
                   "[User/DeactivateUserToken(string username, string usertoken) => A true/false.]";

        [System.Web.Http.AcceptVerbs("GET")]
        public virtual string Get()
        {
            return _response + _availableMethods;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public virtual string Get(int id)
        {
            return $"{_response}, the id parameter was {id}{_availableMethods}";
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public virtual bool CheckConnection()
        {
            return true;
        }

        protected async Task<bool> IsUserInternal(string userId)
        {
            // Get the Id for the Role, Internal.
            string internalRoleId = await db.Roles.Where(r => r.Name == MyRoles.Internal).Select(r => r.Id).FirstOrDefaultAsync();
            // Get the Users that are internal.
            List<User> internalUsers = await db.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(internalRoleId)).ToListAsync();
            // If the user is not in the list of internal users, then return false.
            return internalUsers.Find(u => u.Id == userId) != null;
        }
    }
}
