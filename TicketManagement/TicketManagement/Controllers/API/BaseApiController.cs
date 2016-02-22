using System.Reflection;
using System.Web.Http;

namespace TicketManagement.Controllers.API
{
    public class BaseApiController : ApiController
    {
        private readonly string _response = $"Ticket System API v{Assembly.GetExecutingAssembly().GetName().Version}";

        private readonly string _availableMethods = " : Available API Calls :" +
                   "[Tickets/GetAllTicketsForUser(string username, string usertoken) => Json list of tickets.]" +
                   "[Tickets/GetTicketForUser(string ticketid, string username, string usertoken) => Json list of tickets.]" +
                   "[User/GetNewUserToken(string username) => The user token.]" +
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
    }
}
