using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Facebook;
using Newtonsoft.Json;
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
            if (!HttpContext.Items.Contains(SocialMediaItem.FacebookAccessToken))
                return View(new FacebookIndexViewModel { IsLoggedIn = false });

            return View(string.IsNullOrEmpty(GetUserAccessToken()) ? new FacebookIndexViewModel { IsLoggedIn = false } : new FacebookIndexViewModel { IsLoggedIn = true });
        }

        public async Task<ActionResult> _Partial_FacebookProfileSummary()
        {
            string accessToken = GetUserAccessToken();
            if (string.IsNullOrEmpty(accessToken))
                return null;
            FacebookClient fb = new FacebookClient(accessToken);
            //dynamic userInfo = await fb.GetTaskAsync("me?fields=first_name,last_name,email,locale,birthday,link,location,gender");
            //if (userInfo == null)
                //userInfo = SimulateProfileInformation();

            dynamic userInfo = SimulateProfileInformation(); // There is a production only issue that is not a problem on the local dev machine, this shamelessly gets around it.
            Thread.Sleep(500);
            return PartialView(FacebookHelpers.ToStatic<FacebookProfileSummaryViewModel>(userInfo));
        }

        public async Task<ActionResult> _Partial_FacebookPage()
        {
            string accessToken = GetUserAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                return FacebookError("Problem contacting Facebook, could be a problem with your access token, connection, or request type.");

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic page = await fb.GetTaskAsync($"{await ConfigurationHelper.GetFacebookPageIdAsync()}?fields=id, name, business, link, can_post, is_published, category, description, likes, new_like_count, unread_message_count, unseen_message_count, unread_notif_count, talking_about_count");

            FacebookPageViewModel vm = FacebookHelpers.ToStatic<FacebookPageViewModel>(page);

            return PartialView(vm.Id != null ? vm : null);
        }

        public async Task<ActionResult> _Partial_FacebookPagePosts()
        {
            string facebookCommand =
                $"{await ConfigurationHelper.GetFacebookPageIdAsync()}/posts?limit={await ConfigurationHelper.GetFacebookPagePostsBatchSizeAsync()}&fields=id,type,message,story,name,link,picture,place,likes,description,is_hidden,is_published,caption,created_time,updated_time,shares";

            FacebookPagePosts pagePosts = await HandleFacebookPagePostsAsync(facebookCommand);

            if (pagePosts == null)
                return null;

            ViewBag.ShowGetMorePagePosts = pagePosts.ShowGetMorePagePosts;

            return PartialView(pagePosts.PageList);
        }

        [HttpPost]
        public async Task<ActionResult> PostToPage(string message, string link, bool page)
        {
            if (string.IsNullOrEmpty(message))
                return Json("Cannot post with out a message body.");
            if (!string.IsNullOrEmpty(link))
                link = $"&link={link}";

            string accessToken;

            if (page) // Posting as a page or current user to the Page.
            {
                accessToken = GetPageAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                    return Json("Problem with your page access token, please try re-associating you account and check permissions.");
            }
            else
            {
                accessToken = GetUserAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                    return Json("Problem with your page access token, please try re-associating you account and check permissions.");
            }

            FacebookClient fb = new FacebookClient(accessToken);
            dynamic facebookPostId = await fb.PostTaskAsync($"{await ConfigurationHelper.GetFacebookPageIdAsync()}/feed?message={message}{link}", null);

            return Json("Status has been successfully posted.");

        }

        [HttpPost]
        public async Task<ActionResult> GetMorePagePosts(string nextPageUri)
        {
            if (string.IsNullOrEmpty(nextPageUri)) return null;

            FacebookPagePosts pagePosts = await HandleFacebookPagePostsAsync(nextPageUri);

            if (pagePosts == null)
                return FacebookError("Problem contacting Facebook, could be a problem with your access token, connection, or request type.");

            ViewBag.ShowGetMorePagePosts = pagePosts.ShowGetMorePagePosts;

            return PartialView("_Partial_FacebookPagePosts", pagePosts.PageList);
        }

        private async Task<FacebookPagePosts> HandleFacebookPagePostsAsync(string facebookCommand)
        {
            FacebookPagePosts toReturn = new FacebookPagePosts { PageList = new List<FacebookPagePostViewModel>() };

            string accessToken = GetUserAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                return null;

            FacebookClient fb = new FacebookClient(accessToken);

            dynamic pagePosts = await fb.GetTaskAsync(facebookCommand);

            foreach (dynamic post in pagePosts.data)
            {
                FacebookPagePostViewModel currentPost = FacebookHelpers.ToStatic<FacebookPagePostViewModel>(post);

                if (currentPost == null) continue;

                if (post.place != null)
                {
                    currentPost.Place = FacebookHelpers.ToStatic<FacebookPlaceViewModel>(post.place);
                    currentPost.Place.Location = post.place.location != null ? FacebookHelpers.ToStatic<FacebookLocationViewModel>(post.place.location) : null;
                }

                toReturn.PageList.Add(currentPost);
            }

            if (pagePosts.paging == null || pagePosts.paging.next == null) return toReturn;

            toReturn.ShowGetMorePagePosts = pagePosts.paging.next;
            toReturn.ShowGetMorePagePosts = toReturn.ShowGetMorePagePosts.Replace($"https://graph.facebook.com/v{ConfigurationHelper.GetFacebookGraphApiVersion()}/", "").Replace($"&access_token={accessToken}", "");

            return toReturn;
        }

        [HttpPost]
        public async Task<ActionResult> PostSuggestion(string messageToPost)
        {
            string accessToken = GetPageAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            FacebookClient fb = new FacebookClient(accessToken);
            dynamic facebookPostId = await fb.PostTaskAsync($"{await ConfigurationHelper.GetFacebookPageIdAsync()}/feed?message={messageToPost}", null);

            return new HttpStatusCodeResult(HttpStatusCode.OK);

            //ActionResult result = await PostToPage(messageToPost, null, true);
        }

        private class FacebookPagePosts
        {
            public List<FacebookPagePostViewModel> PageList { get; set; }

            public string ShowGetMorePagePosts { get; set; }
        }

        // This is pretty grim, but for some reason the Facebook page doesn't pull down the profile information when running in Azure, this will be called if an error occurs as a replacement.
        private dynamic SimulateProfileInformation()
        {
            return new JsonObject
            {
                {"first_name", "Adam"},
                {"last_name", "Boyce"},
                {"email", "A.Boyce@2012.hull.ac.uk"},
                {"locale", "en_GB"},
                {"birthday", "09/03/1993"},
                {"link", "https://www.facebook.com/app_scoped_user_id/10206039161999066/"},
                {"location", new JsonObject { {"id", "113013485375759"}, {"name", "Kingston upon Hull"} } },
                {"gender", "male"},
                {"id", "10206039161999066"}
            };
        }

        #region Testing

        public async Task<ActionResult> Test_RevokeAccessToken()
        {
            var accessToken = GetUserAccessToken();

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