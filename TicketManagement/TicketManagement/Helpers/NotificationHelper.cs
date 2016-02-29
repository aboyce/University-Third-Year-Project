using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Helpers
{
    public static class NotificationHelper
    {
        private const int TicketTitleLengthLimit = 50;

        #region Perform Action on the Notifications

        public static async Task<bool> UndertakeNotificationAsync(ApplicationContext db, UserManager<User> um, UserNotification un = null, RoleNotification rn = null)
        {
            if (un != null)
            {
                switch (un.Type)
                {
                    case UserNotificationType.NewTicketLog:
                    {
                        return true; // There is nothing to do in this case, it is just letting the user know.
                    }
                }
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

        public static async Task<bool> RemoveNotificationAsync(ApplicationContext db, UserManager<User> um, UserNotification un = null, RoleNotification rn = null)
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

        #endregion

        #region Add New Notifications for a Ticket Log

        public static async Task<bool> CreateNotificationsForNewTicketLog(Ticket ticket, TicketLog ticketLog, ApplicationContext db)
        {
            if (ticket == null || ticketLog == null || db == null)
                return false;

            if (!string.IsNullOrEmpty(ticket.UserAssignedToId)) // The ticket has a single user we can notify of a new Ticket Log.
            {
                User userAssignedTo = await db.Users.FirstOrDefaultAsync(u => u.Id == ticket.UserAssignedToId);

                if (userAssignedTo == null)
                    return false;

                UserNotification notification = new UserNotification
                {
                    Message = $"New message on your ticket '{(ticket.Title.Length > TicketTitleLengthLimit ? ticket.Title.Substring(0, TicketTitleLengthLimit) : ticket.Title)}' by {ticketLog.SubmittedByUser.FullName} at {ticketLog.TimeOfLog}.",
                    NotificationAboutId = userAssignedTo.Id,
                    NotificationAbout = userAssignedTo,
                    Type = UserNotificationType.NewTicketLog
                };

                return await Task.Run(() => AddUserNotificationToDb(db, notification));
            }
            else // If there is not a User assigned to the Ticket, then send it to all users in the Team that is assigned to the Ticket.
            {
                Team team = await db.Teams.FirstOrDefaultAsync(t => t.Id == ticket.TeamAssignedToId);

                if (team == null)
                    return false;

                List<User> usersInTeam = await db.Users.Where(u => u.TeamId == team.Id).Select(u => u).ToListAsync();

                foreach (User user in usersInTeam)
                {
                    UserNotification notification = new UserNotification
                    {
                        Message = $"New message on your teams ticket '{(ticket.Title.Length > TicketTitleLengthLimit ? ticket.Title.Substring(0, TicketTitleLengthLimit) : ticket.Title)}' by {ticketLog.SubmittedByUser.FullName} at {ticketLog.TimeOfLog}.",
                        NotificationAboutId = user.Id,
                        NotificationAbout = user,
                        Type = UserNotificationType.NewTicketLog
                    };

                    await Task.Run(() => AddUserNotificationToDb(db, notification));
                }
            }

            return true;
        }

        #endregion

        #region Add Notifications to the Database

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

        #endregion

        #region Get Notifications for Users

        public static bool AnyNotificationsForUser(ApplicationContext db, string userId, IList<string> userRolesByName)
        {
            return db.UserNotifications.Any(un => un.NotificationAboutId == userId) || (from rn in db.RoleNotifications.ToList() from roleId in GetRoleIdsForUser(db, userRolesByName) where rn.RoleId == roleId select rn).Any();
        }

        public static List<UserNotification> GetUserNotificationsForUser(ApplicationContext db, string userId)
        {
            return db.UserNotifications.Where(un => un.NotificationAbout.Id == userId).Include(un => un.NotificationAbout).ToList();
        }
        public static List<RoleNotification> GetRoleNotificationsForUser(ApplicationContext db, string userId, IList<string> userRolesByName)
        {
            List<RoleNotification> notifications = new List<RoleNotification>();

            foreach (var rn in db.RoleNotifications.Include(rn => rn.NotificationAbout).ToList())
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

        #endregion

        #region Get Role Ids for a User

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

        #endregion
    }
}
