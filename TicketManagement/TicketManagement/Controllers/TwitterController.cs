using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Helpers;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace TicketManagement.Controllers
{
    public class TwitterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}