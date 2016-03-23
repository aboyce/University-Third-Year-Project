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
        public ActionResult Index()
        {
            ITwitterCredentials twitterCredentials = GetTwitterCredentials();

            return View(twitterCredentials == null ? new TwitterIndexViewModel { IsLoggedIn = false } : new TwitterIndexViewModel { IsLoggedIn = true });
        }

        public async Task<ActionResult> _Partial_TwitterProfileSummary()
        {
            if (!SetTwitterCredentials())
                return null;
            if (!Auth.Credentials.AreSetupForUserAuthentication())
                return null;

            IAuthenticatedUser twitterUser = await Tweetinvi.UserAsync.GetAuthenticatedUser(Auth.Credentials);

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

        public async Task<ActionResult> _Partial_TwitterHomeTimeline()
        {
            if (!SetTwitterCredentials())
                return null;
            if (!Auth.Credentials.AreSetupForUserAuthentication())
                return null;

            return PartialView(GetTweetList(await (await UserAsync.GetAuthenticatedUser(Auth.Credentials)).GetHomeTimelineAsync()));
        }

        public async Task<ActionResult> _Partial_TwitterUserTimeline()
        {
            if (!SetTwitterCredentials())
                return null;
            if (!Auth.Credentials.AreSetupForUserAuthentication())
                return null;

            return PartialView(GetTweetList(await (await UserAsync.GetAuthenticatedUser(Auth.Credentials)).GetUserTimelineAsync()));
        }

        private static List<TwitterTweetViewModel> GetTweetList(IEnumerable<ITweet> tweetsFromTwitter)
        {
            if (tweetsFromTwitter == null)
                return new List<TwitterTweetViewModel>();

            return tweetsFromTwitter.Select(tweet => new TwitterTweetViewModel
            {
                Text = tweet.Text,
                CreatedAt = tweet.CreatedAt,
                CreatedBy = tweet.CreatedBy,
                FavouriteCount = tweet.FavoriteCount,
                HashtagCount = tweet.Hashtags.Count,
                TweetLength = tweet.PublishedTweetLength,
            }).ToList();
        }

        private static List<TwitterTweetViewModel> GetTweetListWithReplies(IEnumerable<ITweet> tweetsFromTwitter)
        {
            if (tweetsFromTwitter == null)
                return new List<TwitterTweetViewModel>();

            List<TwitterTweetViewModel> allTweets= new List<TwitterTweetViewModel>();

            foreach (ITweet tweet in tweetsFromTwitter)
            {
                allTweets.Add(new TwitterTweetViewModel
                {
                    Text = tweet.Text,
                    CreatedAt = tweet.CreatedAt,
                    CreatedBy = tweet.CreatedBy,
                    FavouriteCount = tweet.FavoriteCount,
                    HashtagCount = tweet.Hashtags.Count,
                    TweetLength = tweet.PublishedTweetLength,
                    IsReply = tweet.InReplyToStatusId != null
                });
            }






            return tweetsFromTwitter.Select(tweet => new TwitterTweetViewModel
            {
                Text = tweet.Text,
                CreatedAt = tweet.CreatedAt,
                CreatedBy = tweet.CreatedBy,
                FavouriteCount = tweet.FavoriteCount,
                HashtagCount = tweet.Hashtags.Count,
                TweetLength = tweet.PublishedTweetLength,
            }).ToList();
        }

        public bool SetTwitterCredentials()
        {
            if (!HttpContext.Items.Contains(SocialMediaItem.TwitterAccessToken) || !HttpContext.Items.Contains(SocialMediaItem.TwitterAccessTokenSecret))
                return false; // TODO: Handle this error

            string twitterAccessToken = HttpContext.Items[SocialMediaItem.TwitterAccessToken].ToString();
            string twitterAccessTokenSecret = HttpContext.Items[SocialMediaItem.TwitterAccessTokenSecret].ToString();

            if (string.IsNullOrEmpty(twitterAccessToken) || string.IsNullOrEmpty(twitterAccessTokenSecret))
            {
                // TODO: Handle this error.
                return false;
            }

            ITwitterCredentials credentials =  Auth.SetUserCredentials(ConfigurationHelper.GetTwitterConsumerKey(),ConfigurationHelper.GetTwitterConsumerSecret(), twitterAccessToken, twitterAccessTokenSecret);

            return credentials.AreSetupForUserAuthentication();
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