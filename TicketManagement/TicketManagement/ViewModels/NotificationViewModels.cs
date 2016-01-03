using System.Collections.Generic;
using TicketManagement.Management;

namespace TicketManagement.ViewModels
{
    class NotificationViewModels
    {
    }

    public class NotificationViewModel
    {
        public List<UserNotification> UserNotifications { get; set; }

        public List<RoleNotification> RoleNotifications { get; set; }
    }
}
