using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Facebook;
using TicketManagement.Filters;
using TicketManagement.Management;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    [FacebookAccessToken]
    public class FacebookController : Controller
    {
        public async Task<ActionResult> Index()
        {
            string accessToken = HttpContext.Items["access_token"].ToString();

            if (string.IsNullOrEmpty(accessToken))
                return View(); // Account not associated!

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic userInfo = await fb.GetTaskAsync("me?fields=first_name,last_name,link,locale,email,name,birthday,gender,location,bio,age_range");

            

            return View();
        }
    }
}