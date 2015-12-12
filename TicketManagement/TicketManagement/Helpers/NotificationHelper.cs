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
            db.SaveChangesAsync();

            return true;
        }

        public static bool AddRoleNotificationToDb(ApplicationContext db, RoleNotification notification)
        {
            db.RoleNotifications.Add(notification);
            db.SaveChangesAsync();

            return true;
        }

        public static List<UserNotification> GetUserNotificationsForUser(ApplicationContext db, string userId)
        {
            return db.UserNotifications.Where(userNotification => userNotification.UserIdNotificationOn == userId).ToList();
        }

        public static List<RoleNotification> GetRoleNotificationsForUser(ApplicationContext db, IList<string> userRolesByName, string userId)
        {
            List<string> roleIdsForUserId = GetRoleIdsForUser(db, userRolesByName);

            //foreach (var roleNotification in db.RoleNotifications)
            //{
            //    foreach (var roleId in roleIdsForUserId)
            //    {
            //        if (roleNotification.RoleId == roleId)
            //            notifications.Add(roleNotification);
            //    }
            //}


            return (from roleNotification in db.RoleNotifications from roleId in roleIdsForUserId where roleNotification.RoleId == roleId select roleNotification).ToList();
        }


        private static List<string> GetRoleIdsForUser(ApplicationContext db, IList<string> userRolesByName)
        {
            List<string> roleIdsForUserId = new List<string>();

            foreach (var roleName in userRolesByName)
            {
                foreach (var role in db.Roles)
                {
                    if(role.Name == roleName)
                        roleIdsForUserId.Add(role.Name);
                }
            }

            return roleIdsForUserId;
        }
    }
}
