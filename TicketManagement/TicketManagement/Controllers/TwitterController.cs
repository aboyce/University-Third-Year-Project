using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.ViewModels;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    [TwitterAccessTokens
        ]
    public class TwitterController : Controller
    {
        public ActionResult Index()
        {
            if (!HttpContext.Items.Contains(SocialMediaItem.TwitterVerifierCode))
                return View(new TwitterIndexViewModel {IsLoggedIn = false} );



            return View();
        }
    }
}