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
}
