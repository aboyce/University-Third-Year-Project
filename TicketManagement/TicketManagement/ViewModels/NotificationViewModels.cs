using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Management;

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
