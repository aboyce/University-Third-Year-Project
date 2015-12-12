using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;

namespace TicketManagement.Models.Management
{
    public static class NotificationMessages
    {
        private static readonly Dictionary<NotificationType, string> NotificationMessage = new Dictionary<NotificationType, string>
        {
            { NotificationType.PendingApproval, "The user is awaiting approval before being allowed to login." },
            { NotificationType.PendingInternalApproval, "The user is awaiting approval to be confirmed as an internal user." }
        };

        public static string GetMessageOrNull(NotificationType type)
        {
            string notificationMessage;

            NotificationMessage.TryGetValue(type, out notificationMessage);

            return notificationMessage.IsNullOrWhiteSpace() ? null : notificationMessage;
        }
    }
}
