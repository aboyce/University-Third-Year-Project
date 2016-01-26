using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Facebook;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    [FacebookAccessToken]
    public class FacebookController : Controller
    {
        public ActionResult Index()
        {
            if (!HttpContext.Items.Contains("access_token"))
                return View(new FacebookIndexViewModel { IsLoggedIn = false });

            string accessToken = HttpContext.Items["access_token"].ToString();

            return View(string.IsNullOrEmpty(accessToken) ? new FacebookIndexViewModel { IsLoggedIn = false } : new FacebookIndexViewModel { IsLoggedIn = true });
        }

        public async Task<ActionResult> _Partial_FacebookProfileSummary()
        {
            string accessToken = GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                return PartialView("Error");

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic userInfo = await fb.GetTaskAsync("me?fields=first_name,last_name,email,locale,birthday,link,location");

            return PartialView(FacebookHelpers.ToStatic<FacebookProfileSummaryViewModel>(userInfo));
        }

        public async Task<ActionResult> _Partial_FacebookAdminPage()
        {
            string accessToken = GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                return PartialView("Error");

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic userPages = await fb.GetTaskAsync("me/accounts?fields=id, name, link, is_published, likes, talking_about_count");

            foreach (dynamic page in userPages.data)
            {
                FacebookAdminPageViewModel vm = FacebookHelpers.ToStatic<FacebookAdminPageViewModel>(page);

                if (vm.Id == await ConfigurationHelper.GetFacebookPageIdAsync())
                    return PartialView(vm);
            }

            return PartialView(null);
        }

        public string GetAccessToken()
        {
            if (!HttpContext.Items.Contains("access_token"))
            {
                Error("Cannot find your access token, please try re-associating you account with Facebook");
                return null;
            }

            string accessToken = HttpContext.Items["access_token"].ToString();

            if (string.IsNullOrEmpty(accessToken))
            {
                Error("Cannot find your access token, please try re-associating you account with Facebook");
                return null;
            }

            return accessToken;
        }

        public ActionResult Error(string errorMessage)
        {
            ViewBag.ErrorMessage = "Cannot find your access token, please try re-associating you account with Facebook";
            return PartialView("Error");
        }
    }
}