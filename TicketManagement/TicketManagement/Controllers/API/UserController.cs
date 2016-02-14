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
    public class UserController : BaseApiController
    {
        private ApplicationContext db = new ApplicationContext();

        public string GetUserToken(string username)
        {
            
            return null;
        }





    }
}
