using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Microsoft.AspNet.Identity.Owin;
using TicketManagement.Management;
using TicketManagement.Models.Entities;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    public class FacebookController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var accessToken = HttpContext.Items["access_token"].ToString();

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic myInfo = await fb.GetTaskAsync("me?fields=first_name,last_name,link,locale,email,name,birthday,gender,location,bio,age_range");

            

            return View();
        }
    }

    public class FacebookAccessTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ApplicationUserManager _userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<User>();

            if (_userManager != null)
            {

            }

            base.OnActionExecuting(filterContext);
        }
    }
}