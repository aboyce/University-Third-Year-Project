using System;
using System.Collections.Generic;

namespace TicketManagement.ViewModels
{
    class FacebookViewModels
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FacebookMapping : Attribute
    {
        private string name;
        public string Parent;

        public FacebookMapping(string name)
        {
            this.name = name;
            this.Parent = string.Empty;
        }

        public string GetName()
        {
            return this.name;
        }
    }

    public class FacebookIndexViewModel
    {
        public bool IsLoggedIn { get; set; } = false;
    }

    public class FacebookProfileSummaryViewModel
    {
        [FacebookMapping("first_name")]
        public string FirstName { get; set; }

        [FacebookMapping("last_name")]
        public string LastName { get; set; }

        [FacebookMapping("name")]
        public string FullName { get; set; }

        [FacebookMapping("email")]
        public string Email { get; set; }

        [FacebookMapping("locale")]
        public string Locale { get; set; }

        [FacebookMapping("name", Parent = "location")]
        public string Location { get; set; }

        [FacebookMapping("birthday")]
        public DateTime Birthday { get; set; }

        [FacebookMapping("gender")]
        public string Gender { get; set; }

        [FacebookMapping("link")]
        public string ExternalLink { get; set; }
    }

    public class FacebookPageViewModel
    {
        [FacebookMapping("id")]
        public string Id { get; set; }

        [FacebookMapping("name")]
        public string Name { get; set; }

        [FacebookMapping("description")]
        public string Description { get; set; }

        [FacebookMapping("is_published")]
        public bool Published { get; set; }

        [FacebookMapping("can_post")]
        public bool CanPost { get; set; }

        [FacebookMapping("category")]
        public string Category { get; set; }

        [FacebookMapping("business")]
        public string Business { get; set; }

        [FacebookMapping("likes")]
        public long Likes { get; set; }

        [FacebookMapping("new_like_count")]
        public long NewLikes { get; set; }

        [FacebookMapping("talking_about_count")]
        public long TalkingCount { get; set; }

        [FacebookMapping("unread_message_count")]
        public long UnreadMessages { get; set; }

        [FacebookMapping("unseen_message_count")]
        public long UnseenMessages { get; set; }

        [FacebookMapping("unread_notif_count")]
        public long UnreadNotifications { get; set; }

        [FacebookMapping("link")]
        public string ExternalLink { get; set; }
    }

    public class FacebookPagePostViewModel
    {
        [FacebookMapping("id")]
        public string Id { get; set; }

        [FacebookMapping("type")]
        public string Type { get; set; }

        [FacebookMapping("message")]
        public string Message { get; set; }

        [FacebookMapping("story")]
        public string Story { get; set; }

        [FacebookMapping("name")]
        public string LinkName { get; set; }

        [FacebookMapping("link")]
        public string Link { get; set; }

        [FacebookMapping("picture")]
        public string Image { get; set; }

        [FacebookMapping("description")]
        public string Description { get; set; }

        [FacebookMapping("place")]
        public FacebookPlaceViewModel Place { get; set; }

        [FacebookMapping("is_hidden")]
        public bool Hidden { get; set; }

        [FacebookMapping("is_published")]
        public bool Published { get; set; }

        [FacebookMapping("caption")]
        public string Caption { get; set; }

        [FacebookMapping("created_time")]
        public DateTime Created { get; set; }

        [FacebookMapping("updated_time")]
        public DateTime Updated { get; set; }
    }

    public class FacebookPlaceViewModel
    {
        [FacebookMapping("id")]
        public string Id { get; set; }

        [FacebookMapping("name")]
        public string Message { get; set; }

        [FacebookMapping("location")]
        public FacebookLocationViewModel Location { get; set; }
    }

    public class FacebookLocationViewModel
    {
        [FacebookMapping("city")]
        public string City { get; set; }

        [FacebookMapping("country")]
        public string Country { get; set; }

        [FacebookMapping("longitude")]
        public double Longitude { get; set; }

        [FacebookMapping("latitude")]
        public double Latitude { get; set; }

        [FacebookMapping("zip")]
        public string PostCode { get; set; }
    }
}
