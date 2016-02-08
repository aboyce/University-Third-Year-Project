using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.ViewModels;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    [TwitterAccessTokens]
    public class TwitterController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ITwitterCredentials twitterCredentials = GetTwitterCredentials();

            return View(twitterCredentials == null ? new TwitterIndexViewModel { IsLoggedIn = false } : new TwitterIndexViewModel { IsLoggedIn = true });
        }

        public async Task<ActionResult> _Partial_TwitterProfileSummary()
        {
            ITwitterCredentials twitterCredentials = GetTwitterCredentials();

            if (twitterCredentials == null)
                return null;

            ILoggedUser twitterUser = Auth.ExecuteOperationWithCredentials(twitterCredentials, Tweetinvi.User.GetLoggedUser);

            return PartialView(new TwitterProfileSummaryViewModel
            {
                ScreenName = twitterUser.ScreenName,
                Name = twitterUser.Name,
                ProfileImageUrl = twitterUser.ProfileImageUrl400x400,
                FavouritesCount = twitterUser.FavouritesCount,
                FollowersCount = twitterUser.FollowersCount,
                FriendsCount = twitterUser.FriendsCount            
            });
        }



        public ITwitterCredentials GetTwitterCredentials()
        {
            if (!HttpContext.Items.Contains(SocialMediaItem.TwitterAccessToken) || !HttpContext.Items.Contains(SocialMediaItem.TwitterAccessTokenSecret))
                return null; // TODO: Handle this error

            string twitterAccessToken = HttpContext.Items[SocialMediaItem.TwitterAccessToken].ToString();
            string twitterAccessTokenSecret = HttpContext.Items[SocialMediaItem.TwitterAccessTokenSecret].ToString();

            if (string.IsNullOrEmpty(twitterAccessToken) || string.IsNullOrEmpty(twitterAccessTokenSecret))
            {
                // TODO: Handle this error.
                return null;
            }

            return new TwitterCredentials(ConfigurationHelper.GetTwitterConsumerKey(), ConfigurationHelper.GetTwitterConsumerSecret(), twitterAccessToken, twitterAccessTokenSecret);
        }
    }
}