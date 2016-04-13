using System.Collections.Generic;
using TicketManagement.Management;

namespace TicketManagement.ViewModels
{
    class NotificationViewModels
    {
    }

    public class NotificationViewModel
    {
        public bool Notifications { get; set; }
    }

    public class UserNotificationViewModel
    {
        public List<UserNotification> UserNotifications { get; set; }
    }

    public class RoleNotificationViewModel
    {
        public List<RoleNotification> RoleNotifications { get; set; }
    }

    public class SocialMediaNotificationsViewModel
    {
        public SocialMediaNotificationsViewModel(List<SocialMediaNotificationViewModel> socialMediaNotifications)
        {
            SocialMediaNotifications = socialMediaNotifications;
        }

        public List<SocialMediaNotificationViewModel> SocialMediaNotifications { get; set; }
    }

    public class SocialMediaNotificationViewModel
    {
        public SocialMediaNotificationViewModel(int notificationId, string messageToPost)
        {
            NotificationId = notificationId;
            MessageToPost = messageToPost;
        }

        public int NotificationId { get; set; }

        public string MessageToPost { get; set; }
    }
}
