using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public static async Task<bool> UndertakeNotificationAsync(ApplicationContext db, UserManager<User> um, UserNotification un = null, RoleNotification rn = null)
        {
            if (un != null)
            {
                // Currently no User Notifications
            }
            else if (rn != null)
            {

                switch (rn.Type)
                {
                    case RoleNotificationType.PendingApproval:
                        {
                            if (!um.IsInRole(rn.NotificationAboutId, MyRoles.Approved))
                                um.AddToRole(rn.NotificationAboutId, MyRoles.Approved);

                            db.RoleNotifications.Remove(rn);
                            await db.SaveChangesAsync();

                            return true;
                        }
                    case RoleNotificationType.PendingInternalApproval:
                        {
                            if (!um.IsInRole(rn.NotificationAboutId, MyRoles.Internal))
                                um.AddToRole(rn.NotificationAboutId, MyRoles.Internal);

                            db.RoleNotifications.Remove(rn);
                            await db.SaveChangesAsync();

                            return true;
                        }
                }
            }

            return false;
        }

        public static async Task<bool> DeclineNotificationAsync(ApplicationContext db, UserManager<User> um, UserNotification un = null, RoleNotification rn = null)
        {
            if (un != null)
            {
                db.UserNotifications.Remove(un);
                await db.SaveChangesAsync();

                return true;
            }
            else if (rn != null)
            {
                db.RoleNotifications.Remove(rn);
                await db.SaveChangesAsync();

                return true;
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

        /* GET NOTIFICATIONS FOR USERS */
        public static List<UserNotification> GetUserNotificationsForUser(ApplicationContext db, string userId)
        {
            return  db.UserNotifications.Where(un => un.NotificationAbout.Id == userId).ToList();
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
