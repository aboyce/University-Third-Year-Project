using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;

namespace TicketManagement.Helpers
{
    public static class NotificationHelper
    {
        /* PERFORM THE ACTION ON THE NOTIFICATION */
        public static bool UndertakeNotification(ApplicationContext db, UserManager<ApplicationUser> um, UserNotification un = null, RoleNotification rn = null)
        {
            if (un != null)
            {
                un = FillInMissingClasses(db, un); // Currently no UserNotifications
            }
            else if (rn != null)
            {
                rn = FillInMissingClasses(db, rn);

                switch (rn.Type)
                {
                    case RoleNotificationType.PendingApproval:
                        {
                            um.AddToRole(rn.NotificationAboutId, "Approved");
                            db.RoleNotifications.Remove(rn);
                            db.SaveChanges();
                            return true;
                        }
                    case RoleNotificationType.PendingInternalApproval:
                        {
                            um.AddToRole(rn.NotificationAboutId, "Internal");
                            db.RoleNotifications.Remove(rn);
                            db.SaveChanges();
                            return true;
                        }
                }
            }

            return false;
        }

        /* ADD NOTIFICATIONS TO THE DATABASE */
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

        /* FILL IN MISSING CLASSES */
        public static UserNotification FillInMissingClasses(ApplicationContext db, UserNotification un)
        {
            if (un.NotificationAbout == null)
                un.NotificationAbout = db.Users.FirstOrDefault(u => u.Id == un.NotificationAboutId);

            return un;
        }
        public static RoleNotification FillInMissingClasses(ApplicationContext db, RoleNotification rn)
        {
            if (rn.NotificationAbout == null)
                rn.NotificationAbout = db.Users.FirstOrDefault(u => u.Id == rn.NotificationAboutId);

            if (rn.Role == null)
                rn.Role = db.Roles.FirstOrDefault(r => r.Id == rn.RoleId);

            return rn;
        }

        /* GET NOTIFICATIONS FOR USERS */
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

        /* GET ROLE IDS FOR A USER */
        private static IEnumerable<string> GetRoleIdsForUser(ApplicationContext db, IEnumerable<string> userRolesByName)
        {
            List<string> roleIdsForUserId = new List<string>();

            foreach (var roleName in userRolesByName)
            {
                foreach (var role in db.Roles)
                {
                    if (role.Name == roleName)
                        roleIdsForUserId.Add(role.Id);
                }
            }

            return roleIdsForUserId;
        }
    }
}
