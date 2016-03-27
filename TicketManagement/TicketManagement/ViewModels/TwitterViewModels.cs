using System;
using System.Collections.Generic;

namespace TicketManagement.ViewModels
{
    class TwitterViewModels
    {
    }

    public class TwitterIndexViewModel
    {
        public bool IsLoggedIn { get; set; } = false;
    }

    public class TwitterProfileSummaryViewModel
    {
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public int FavouritesCount { get; set; }
        public int FollowersCount { get; set; }
        public int FriendsCount { get; set; }
        public string ProfileImageUrl { get; set; }
    }

    public class TwitterTweetViewModel
    {
        public long TwitterId { get; set; } // The Id given from the Twitter API.
        public string Text { get; set; }
        public Tweetinvi.Core.Interfaces.IUser CreatedBy { get; set; } = null;
        public DateTime CreatedAt { get; set; }
        public int FavouriteCount { get; set; }
        public int HashtagCount { get; set; }
        public int TweetLength { get; set; }
        public long? ReplyToTwitterId { get; set; } // If its a reply, the Id from the Twitter API of the Tweet it is a reply to.
        public List<TwitterTweetViewModel> Replies { get; set; } = new List<TwitterTweetViewModel>();
        public int? LinkedTicketId { get; set; } = null; // Null if does not contain the hashtag. -1 if not a Ticket, TicketId if a Ticket exists.
    }
}
