using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TicketManagement.Models.Context;

using ApiController = System.Web.Http.ApiController;
using AcceptVerbsAttribute = System.Web.Http.AcceptVerbsAttribute;

namespace TicketManagement.Controllers.API
{
    [System.Web.Mvc.AllowAnonymous]
    public class TicketsController : BaseApiController
    {
        private ApplicationContext db = new ApplicationContext();

        [AcceptVerbs("GET")]
        public JsonResult Test()
        {
            var result = new JsonResult
            {
                Data = new
                {
                    Name = "Jonny Boy"
                }
            };
            return result;
        }

        //[AcceptVerbs("GET")]
        //public JsonResult GetAllUsers()
        //{
        //    var users = new List<int> { 3, 4, 2, 4, 65, 545, 22 };

        //    return Json(users);
        //}


    }
}
