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
using Tweetinvi.Core.Exceptions;
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

        public ActionResult TwitterError(string errorMessage)
        {
            ViewBag.Type = "Twitter";
            ViewBag.ErrorMessage = errorMessage;
            return PartialView("_Partial_SocialMediaNotLoggedIn");
        }

        public ActionResult _Partial_TwitterProfileSummary()
        {
            try
            {
                ITwitterCredentials credentials = GetTwitterCredentials();
                if (credentials == null)
                    return null;

                IAuthenticatedUser twitterUser = Tweetinvi.User.GetAuthenticatedUser(credentials, null);

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
            catch (TwitterException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ActionResult> _Partial_TwitterHomeTimeline()
        {
            try
            {
                ITwitterCredentials credentials = GetTwitterCredentials();
                if (credentials == null)
                    return null;

                return PartialView("_Partial_TwitterTimeline", GetTweetListWithReplies(await (Tweetinvi.User.GetAuthenticatedUser(credentials)).GetHomeTimelineAsync()));
            }
            catch (TwitterException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ActionResult> _Partial_TwitterUserTimeline()
        {
            try
            {
                ITwitterCredentials credentials = GetTwitterCredentials();
                if (credentials == null)
                    return TwitterError("Problem loading your credentials.");

                return PartialView("_Partial_TwitterTimeline", GetTweetListWithReplies(await (Tweetinvi.User.GetAuthenticatedUser(credentials)).GetUserTimelineAsync()));
            }
            catch (TwitterException e)
            {
                return TwitterError(e.TwitterDescription);
            }
            catch (Exception e)
            {
                return TwitterError(e.Message);
            }
        }

        private static List<TwitterTweetViewModel> GetTweetListWithReplies(IEnumerable<ITweet> tweetsFromTwitter)
        {
            if (tweetsFromTwitter == null) return null;
            List<TwitterTweetViewModel> tweetViewModels = tweetsFromTwitter.Select(tweet => new TwitterTweetViewModel
            {
                TwitterId = tweet.Id,
                Text = tweet.Text,
                CreatedAt = tweet.CreatedAt,
                CreatedBy = tweet.CreatedBy,
                FavouriteCount = tweet.FavoriteCount,
                HashtagCount = tweet.Hashtags.Count,
                TweetLength = tweet.PublishedTweetLength,
                ReplyToTwitterId = tweet.InReplyToStatusId,
                TicketRequest = tweet.Hashtags.Exists(ht => string.Equals(ht.Text, ConfigurationHelper.GetTwitterHashtag(), StringComparison.CurrentCultureIgnoreCase))            
            }).ToList();

            // This should add the replies to the corresponding parent Tweet.
            foreach (TwitterTweetViewModel tweet in tweetViewModels)
            {
                if (tweet.ReplyToTwitterId == null) continue; // If it isn't a reply to another Tweet, we can just move on.
                foreach (TwitterTweetViewModel parentTweet in tweetViewModels.Where(parentTweet => tweet.ReplyToTwitterId == parentTweet.TwitterId))
                { parentTweet.Replies.Add(tweet); break; } // If it is a reply to a Tweet, we need to find its parent and add it to it's list of replies.
            }
            // Remove the non-parents as they should be now added as replies.
            tweetViewModels.RemoveAll(t => t.ReplyToTwitterId != null);

            return tweetViewModels;
        }

        public bool SetTwitterCredentials()
        {
            if(Auth.Credentials.AreSetupForUserAuthentication())
                return true;


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