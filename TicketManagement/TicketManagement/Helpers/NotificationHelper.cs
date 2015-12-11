using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;

namespace TicketManagement.Helpers
{
    public static class NotificationHelper
    {
        public static List<NotificationCategory> CheckForNotificationsForUser(ApplicationContext db, IList<string> userRolesByName, string userId)
        {
            List<NotificationCategory> notifications = new List<NotificationCategory>();

            if (db.UserNotifications.Count(un => un.UserIdNotificationOn == userId) > 0)
                notifications.Add(NotificationCategory.User);

            var roleIdsForUserId = (from roleName in userRolesByName from role in db.Roles where roleName == role.Name select db.Roles.Where(r => r.Name == role.Name).Select(r => r.Id).FirstOrDefault()).ToList();

            if (roleIdsForUserId.Any(role => db.RoleNotifications.Count(rn => rn.RoleId == role) > 0))
            {
                notifications.Add(NotificationCategory.Role);
                return notifications;
            }

            return notifications;
        }
    }
}
