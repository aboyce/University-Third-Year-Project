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

    public class FacebookAdminPageViewModel
    {
        [FacebookMapping("id")]
        public string Id { get; set; }

        [FacebookMapping("name")]
        public string Name { get; set; }

        [FacebookMapping("likes")]
        public long Likes { get; set; }

        [FacebookMapping("talking_about_count")]
        public long TalkingCount { get; set; }

        [FacebookMapping("is_published")]
        public bool Published { get; set; }

        [FacebookMapping("category")]
        public string Category { get; set; }

        [FacebookMapping("business")]
        public string Business { get; set; }

        [FacebookMapping("can_post")]
        public bool CanPost { get; set; }

        [FacebookMapping("unread_message_count")]
        public long UnreadMessages { get; set; }

        [FacebookMapping("unseen_message_count")]
        public long UnseenMessages { get; set; }

        [FacebookMapping("unread_notif_count")]
        public long UnreadNotifications { get; set; }

        [FacebookMapping("link")]
        public string ExternalLink { get; set; }
    }

    public class FacebookPermissionRequestViewModel
    {
        public FacebookPermissionRequestViewModel()
        {
            MissingPermissions = new List<FacebookPermissionRequest>();
        }
        public List<FacebookPermissionRequest> MissingPermissions { get; set; }
    }

    public class FacebookPermissionRequest
    {
        public bool Requested { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PermissionScope { get; set; }

        public bool  Granted { get; set; }
    }
}
