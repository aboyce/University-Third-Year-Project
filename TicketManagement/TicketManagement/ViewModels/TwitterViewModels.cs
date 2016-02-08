using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
