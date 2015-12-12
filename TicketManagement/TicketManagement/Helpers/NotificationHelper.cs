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
        public static bool AddUserNotificationToDb(ApplicationContext db, UserNotification notification)
        {
            db.UserNotifications.Add(notification);
            db.SaveChanges();

            return true;
        }

        public static bool AddRoleNotificationToDb(ApplicationContext db, RoleNotification notification)
        {
            db.RoleNotifications.Add(notification);
            db.SaveChanges();

            return true;
        }

        public static List<UserNotification> GetUserNotificationsForUser(ApplicationContext db, string userId)
        {
            return db.UserNotifications.Where(un => un.NotificationAbout.Id == userId).ToList();
        }

        public static List<RoleNotification> GetRoleNotificationsForUser(ApplicationContext db, string userId, IList<string> userRolesByName)
        {
            List<RoleNotification> notifications = new List<RoleNotification>();

            foreach (var rn in db.RoleNotifications.ToList())
            {
                foreach (var roleId in GetRoleIdsForUser(db, userRolesByName))
                {
                    if (rn.Role.Id == roleId)
                    {
                        if (rn.NotificationAbout == null)
                            rn.NotificationAbout = db.Users.FirstOrDefault(u => u.Id == rn.NotificationAboutId);

                        notifications.Add(rn);
                    }
                }
            }

            return notifications;
        }


        private static List<string> GetRoleIdsForUser(ApplicationContext db, IList<string> userRolesByName)
        {
            List<string> roleIdsForUserId = new List<string>();

            foreach (var roleName in userRolesByName)
            {
                foreach (var role in db.Roles)
                {
                    if(role.Name == roleName)
                        roleIdsForUserId.Add(role.Id);
                }
            }

            return roleIdsForUserId;
        }
    }
}
