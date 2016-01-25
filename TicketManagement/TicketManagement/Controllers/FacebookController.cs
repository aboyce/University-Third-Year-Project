using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Facebook;
using TicketManagement.Filters;
using TicketManagement.Management;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    [FacebookAccessToken]
    public class FacebookController : Controller
    {
        public async Task<ActionResult> Index()
        {
            if (!HttpContext.Items.Contains("access_token"))
                return View(new FacebookIndexViewModel { IsLoggedIn = false, FacebookProfileSummaryViewModel = null });

            string accessToken = HttpContext.Items["access_token"].ToString();

            if (string.IsNullOrEmpty(accessToken))
                return View(); // Account not associated!

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic userInfo = await fb.GetTaskAsync("me?fields=first_name,last_name,email,locale,birthday,link,location,bio");

            FacebookProfileSummaryViewModel vm = new FacebookProfileSummaryViewModel
            {
                FirstName = userInfo.first_name.ToString(),
                LastName = userInfo.last_name.ToString(),
                Email = userInfo.email.ToString(),
                Locale = userInfo.locale.ToString(),
                Birthday = userInfo.birthday.ToString(),
                Location = userInfo.location.ToString(),
                //Bio = userInfo.bio.ToString(),
                ExternalLink = userInfo.link.ToString()
            };

            return View(new FacebookIndexViewModel { IsLoggedIn = true, FacebookProfileSummaryViewModel = vm });
        }
    }
}