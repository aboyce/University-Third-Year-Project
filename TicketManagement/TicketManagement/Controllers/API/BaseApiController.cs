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

        public virtual string Get()
        {
            return _response;
        }

        public virtual string Get(int id)
        {
            return $"{_response}, the id parameter was {id}";
        }
    }
}
