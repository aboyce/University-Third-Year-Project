using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facebook;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    public class FacebookController : FacebookErrorHandlerController
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
                return FacebookError("");

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic userInfo = await fb.GetTaskAsync("me?fields=first_name,last_name,email,locale,birthday,link,location,gender");

            return PartialView(FacebookHelpers.ToStatic<FacebookProfileSummaryViewModel>(userInfo));
        }

        public async Task<ActionResult> _Partial_FacebookPage()
        {
            string accessToken = GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                return FacebookError("");

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic page = await fb.GetTaskAsync($"{await ConfigurationHelper.GetFacebookPageIdAsync()}?fields=id, name, business, link, can_post, is_published, category, description, likes, new_like_count, unread_message_count, unseen_message_count, unread_notif_count, talking_about_count");

            FacebookPageViewModel vm = FacebookHelpers.ToStatic<FacebookPageViewModel>(page);

            return PartialView(vm.Id != null ? vm : null);
        }

        public async Task<ActionResult> _Partial_FacebookPagePosts()
        {
            string accessToken = GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                return FacebookError("");

            List<FacebookPagePostViewModel> vm = new List<FacebookPagePostViewModel>();

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic pagePosts = await fb.GetTaskAsync($"{await ConfigurationHelper.GetFacebookPageIdAsync()}/posts?fields=id,message,name,link,picture,place,story,likes,description,is_hidden,is_published,caption,created_time,updated_time,shares");

            foreach (dynamic post in pagePosts.data)
            {
                FacebookPagePostViewModel currentPost = FacebookHelpers.ToStatic<FacebookPagePostViewModel>(post);

                if (currentPost == null) continue;

                if (post.place != null)
                {
                    currentPost.Place = FacebookHelpers.ToStatic<FacebookPlaceViewModel>(post.place);
                    currentPost.Place.Location = post.place.location != null ? FacebookHelpers.ToStatic<FacebookLocationViewModel>(post.place.location) : null;
                }
                        
                vm.Add(currentPost);
            }

            return PartialView(vm);
        }

        #region Testing

        public async Task<ActionResult> Test_RevokeAccessToken()
        {
            var accessToken = GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                throw new HttpException(404, "Missing Access Token");

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic userFeed = await fb.DeleteTaskAsync("me/permissions");
            return RedirectToAction("Index", "Tickets");
        }

        public Action Test_ApiLimit()
        {
            throw new FacebookApiLimitException();
        }

        #endregion
    }
}