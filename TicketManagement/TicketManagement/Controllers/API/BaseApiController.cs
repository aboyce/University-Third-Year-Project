using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace TicketManagement.Controllers.API
{
    public class BaseApiController : ApiController
    {
        private readonly string _response = $"Ticket System API v{Assembly.GetExecutingAssembly().GetName().Version}";

        private readonly string _availableMethods = $" : Available API Calls :" +
                   $"{Environment.NewLine}Tickets/GetAllTicketsForUser(string username, string usertoken) => Json list of tickets." +
                   $"{Environment.NewLine}User/GetNewUserToken(string username) => The user token." +
                   $"{Environment.NewLine}User/ClearUserToken(string username, string usertoken) => A true/false.";

        public virtual string Get()
        {
            return _response + _availableMethods;
        }

        public virtual string Get(int id)
        {
            return $"{_response}, the id parameter was {id}{_availableMethods}";
        }
    }
}
