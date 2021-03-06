﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.ViewModels;
using Tweetinvi;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    [TwitterAccessTokens]
    public class TwitterController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public ActionResult Index()
        {
            ITwitterCredentials twitterCredentials = GetTwitterCredentials();
            IUser user = null;

            if(twitterCredentials != null)
                user = Tweetinvi.User.GetAuthenticatedUser(twitterCredentials);

            return View(twitterCredentials == null || user == null ? new TwitterIndexViewModel { IsLoggedIn = false } : new TwitterIndexViewModel { IsLoggedIn = true });
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
                    return TwitterError("Problem loading your credentials.");

                IAuthenticatedUser twitterUser = Tweetinvi.User.GetAuthenticatedUser(credentials);
                if (twitterUser != null)
                {
                    IEnumerable<ITweet> tweets = await twitterUser.GetHomeTimelineAsync();
                    return PartialView("_Partial_TwitterTimeline", GetTweetListWithReplies(tweets));
                }
                else
                    return TwitterError("Problem loading your credentials.");
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

                IAuthenticatedUser twitterUser = Tweetinvi.User.GetAuthenticatedUser(credentials);

                return PartialView("_Partial_TwitterTimeline", GetTweetListWithReplies(await twitterUser.GetUserTimelineAsync()));
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

        [HttpPost]
        public ActionResult AddTweet(string tweetBody, string tweetLink)
        {
            if (string.IsNullOrEmpty(tweetBody))
                return Json("Cannot post with out a message body.");

            try
            {
                ITwitterCredentials credentials = GetTwitterCredentials();
                if (credentials == null)
                    return Json("Problem with your credentials, please try re-associating you account and check permissions.");

                IAuthenticatedUser user = Tweetinvi.User.GetAuthenticatedUser(credentials);
                ITweet tweet = user.PublishTweet(tweetBody + (string.IsNullOrEmpty(tweetLink) ? "" : $" {tweetLink}"));

                if (tweet != null)
                    return Json("Tweet sent!");
                else
                    return Json("Problem with your credentials, please try re-associating you account and check permissions.");
            }
            catch (TwitterException e)
            {
                return Json("Sorry, an error occurred.");
            }
            catch (Exception e)
            {
                return Json("Sorry, an error occurred.");
            }
        }

        [HttpPost]
        public ActionResult AddTwitterReply(string tweet_reply_id, string tweet_reply_body)
        {
            if (string.IsNullOrEmpty(tweet_reply_id) || string.IsNullOrEmpty(tweet_reply_body))
                return RedirectToAction("Index"); // TODO: Add reply was not successful

            tweet_reply_id = tweet_reply_id.Replace("_", ""); // a '_' will most likely have been added by the View for formatting purposes.

            long tweetReplyId;
            
            if(!long.TryParse(tweet_reply_id, out tweetReplyId))
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TwitterReplyFailed });

            try
            {
                ITwitterCredentials credentials = GetTwitterCredentials();
                if (credentials == null)
                    return RedirectToAction("Index", new { ViewMessage = ViewMessage.TwitterReplyFailed });

                string tweetReplyText = $"@{Tweet.GetTweet(tweetReplyId).CreatedBy.ScreenName} {tweet_reply_body}";

                if (Tweet.PublishTweetInReplyTo(tweetReplyText, tweetReplyId) != null)
                    return RedirectToAction("Index", new { ViewMessage = ViewMessage.TwitterReplyAdded });
                else
                    return RedirectToAction("Index", new { ViewMessage = ViewMessage.TwitterReplyFailed });

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

        private List<TwitterTweetViewModel> GetTweetListWithReplies(IEnumerable<ITweet> tweetsFromTwitter)
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
                LinkedTicketId = TicketRequestAvailable(tweet)            
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

        private int? TicketRequestAvailable(ITweet tweet)
        {
            string configuredHastag = ConfigurationHelper.GetTwitterHashtag();
            // First does the Hashtag exist in the tweet
            bool ticketRequest = tweet.Hashtags.Exists(ht => string.Equals(ht.Text, configuredHastag, StringComparison.CurrentCultureIgnoreCase));

            if (!ticketRequest)
                return null;

            // Next see if we have already created a Ticket (we don't want multiple people to create the same Ticket.
            // TODO: This should not be on Ticket Title (as this can be changed and string-string comparison is not great anyway)...
            Ticket relatedTicket = db.Tickets.FirstOrDefault(t => t.Title.Contains(tweet.Text));

            if (relatedTicket == null)
                return -1; // For this case there is not a created Ticket.
            else
                return relatedTicket.Id;
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

            ITwitterCredentials credentials = Auth.SetUserCredentials(ConfigurationHelper.GetTwitterConsumerKey(),ConfigurationHelper.GetTwitterConsumerSecret(), twitterAccessToken, twitterAccessTokenSecret);

            return credentials.AreSetupForUserAuthentication();
        }

        [HttpPost]
        public ActionResult PostSuggestion(string messageToPost)
        {
            try
            {
                ITwitterCredentials credentials = GetTwitterCredentials();
                if (credentials == null)
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

                if (Tweet.PublishTweet(messageToPost) != null)
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            }
            catch (TwitterException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
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

            //return new TwitterCredentials(ConfigurationHelper.GetTwitterConsumerKey(), ConfigurationHelper.GetTwitterConsumerSecret(), twitterAccessToken, twitterAccessTokenSecret);
            return Auth.SetUserCredentials(ConfigurationHelper.GetTwitterConsumerKey(), ConfigurationHelper.GetTwitterConsumerSecret(), twitterAccessToken, twitterAccessTokenSecret);
        }
    }
}