using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Approved)]
    public class NotificationController : Controller
    {
        private ApplicationContext db = new ApplicationContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _Partial_Notifications()
        {
            User user = UserManager.FindById(User.Identity.GetUserId());

            return PartialView(new NotificationViewModel
            {
                Notifications = NotificationHelper.AnyNotificationsForUser(db, user.Id, UserManager.GetRoles(user.Id))
            });
        }

        public async Task<ActionResult> _Partial_UserNotifications()
        {
            User user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            return PartialView(new UserNotificationViewModel
            {
                UserNotifications = NotificationHelper.GetUserNotificationsForUser(db, user.Id)
            });
        }

        public async Task<ActionResult> _Partial_RoleNotifications()
        {
            User user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            return PartialView(new RoleNotificationViewModel()
            {
                RoleNotifications = NotificationHelper.GetRoleNotificationsForUser(db, user.Id, UserManager.GetRoles(user.Id))
            });
        }

        public async Task<ActionResult> _Partial_SocialMedia_Notifications(bool ticketsDay, bool ticketsWeek, bool ticketsMonth, bool ticketsTotal)
        {
            List<SocialMediaNotificationViewModel> socialMediaNotifications = new List<SocialMediaNotificationViewModel>();
            DateTime now = DateTime.Now;
            TicketState closedState = await db.TicketStates.FirstOrDefaultAsync(ts => ts.Name == "Closed");
            if (closedState == null)
                return null;
            double totalAmountOfTickets = await db.Tickets.CountAsync(t => t.TicketState != closedState);

            if (ticketsDay)
            {
                double amountOfTicketsOpenedToday = await db.Tickets.Where(t => t.Created > now.AddDays(-1)).CountAsync();
                double amountOfTicketsClosedToday = await db.Tickets.Where(t => t.TicketState == closedState && t.LastMessage > now.AddDays(-1)).CountAsync();

                if (amountOfTicketsOpenedToday > 0) // Avoid a divide by zero error.
                {
                    double percentageClosed = amountOfTicketsClosedToday / amountOfTicketsOpenedToday;
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel($"We resolved {percentageClosed.ToString("P")} of your issues today!"));
                }
                else
                {
                    if ((int)totalAmountOfTickets != 0) // Avoid a divide by zero error.
                    {
                        double percentageClosed = amountOfTicketsClosedToday / totalAmountOfTickets;
                        socialMediaNotifications.Add(new SocialMediaNotificationViewModel($"We resolved {percentageClosed.ToString("P")} extra, of your issues today!"));
                    }
                }
            }

            if (ticketsWeek)
            {
                double amountOfTicketsOpenedThisWeek = await db.Tickets.Where(t => t.Created > now.AddDays(-7)).CountAsync();
                double amountOfTicketsClosedThisWeek = await db.Tickets.Where(t => t.TicketState == closedState && t.LastMessage > now.AddDays(-7)).CountAsync();

                if (amountOfTicketsOpenedThisWeek > 0) // Avoid a divide by zero error.
                {
                    double percentageClosed = amountOfTicketsClosedThisWeek/amountOfTicketsOpenedThisWeek;
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel($"We resolved {percentageClosed.ToString("P")} of your issues this week!"));
                }
                else
                {
                    if ((int) totalAmountOfTickets != 0) // Avoid a divide by zero error.
                    {
                        double percentageClosed = amountOfTicketsClosedThisWeek / totalAmountOfTickets;
                        socialMediaNotifications.Add(new SocialMediaNotificationViewModel($"We resolved {percentageClosed.ToString("P")} extra, of your issues this week!"));
                    }
                }
            }
            if (ticketsMonth)
            {
                double amountOfTicketsOpenedThisMonth = await db.Tickets.Where(t => t.Created > now.AddMonths(-1)).CountAsync();
                double amountOfTicketsClosedThisMonth = await db.Tickets.Where(t => t.TicketState == closedState && t.LastMessage > now.AddMonths(-1)).CountAsync();

                if (amountOfTicketsOpenedThisMonth > 0) // Avoid a divide by zero error.
                {
                    double percentageClosed = amountOfTicketsClosedThisMonth / amountOfTicketsOpenedThisMonth;
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel($"We resolved {percentageClosed.ToString("P")} of your issues this month!"));
                }
                else
                {
                    if ((int)totalAmountOfTickets != 0) // Avoid a divide by zero error.
                    {
                        double percentageClosed = amountOfTicketsClosedThisMonth / totalAmountOfTickets;
                        socialMediaNotifications.Add(new SocialMediaNotificationViewModel($"We resolved {percentageClosed.ToString("P")} extra, of your issues this month!"));
                    }
                }
            }
            if (ticketsTotal)
            {
                double amountOfTicketsClosed = await db.Tickets.CountAsync(t => t.TicketState == closedState);

                if ((int)totalAmountOfTickets != 0) // Avoid a divide by zero error.
                {
                    double percentageClosed = amountOfTicketsClosed / totalAmountOfTickets;
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel($"So far we have resolved {percentageClosed.ToString("P")} of all of your issues to date!"));
                }
            }

            return null; // TODO: Something with this...
        }

        public async Task<ActionResult> _Partial_SocialMedia_Management_Notifications(bool userTicketsDay, bool userTicketsWeek, bool userTicketsMonth, bool userTicketsTotal,
                                                                                        bool userRepliesDay, bool userRepliesWeek, bool userRepliesMonth, bool userRepliesTotal)
        {
            if (userTicketsDay)
            {

            }
            if (userTicketsWeek)
            {

            }
            if (userTicketsMonth)
            {

            }
            if (userTicketsTotal)
            {

            }
            if (userRepliesDay)
            {

            }
            if (userRepliesWeek)
            {

            }
            if (userRepliesMonth)
            {

            }
            if (userRepliesTotal)
            {

            }

            return null;
        }

        public async Task<ActionResult> AuthoriseNotification(NotificationViewModel vm, NotificationCategory notificationCategory, int notificationId)
        {
            bool success = false;

            if (notificationId > 0)
            {
                if (notificationCategory == NotificationCategory.User)
                {
                    success = await NotificationHelper.UndertakeNotificationAsync(db, UserManager,
                        un: await db.UserNotifications.Include(un => un.NotificationAbout).FirstOrDefaultAsync(un => un.Id == notificationId));
                }
                else if (notificationCategory == NotificationCategory.Role)
                {
                    success = await NotificationHelper.UndertakeNotificationAsync(db, UserManager,
                        rn: await db.RoleNotifications.Include(rn => rn.NotificationAbout).Include(rn => rn.Role).FirstOrDefaultAsync(rn => rn.Id == notificationId));
                }
            }

            return RedirectToAction("Index", success ? new { ViewMessage = ViewMessage.AppliedRoleFromNotification } : new { ViewMessage = ViewMessage.FailedToApplyRoleFromNotification });
        }

        public async Task<ActionResult> DismissNotification(NotificationCategory notificationCategory, int notificationId)
        {
            bool success = false;
            bool newTicketLog = false;

            if (notificationId > 0)
            {
                switch (notificationCategory)
                {
                    case NotificationCategory.User:
                        UserNotification userNotification = await db.UserNotifications.Include(un => un.NotificationAbout).FirstOrDefaultAsync(un => un.Id == notificationId);
                        newTicketLog = userNotification.Type == UserNotificationType.NewTicketLog;
                        success = await NotificationHelper.RemoveNotificationAsync(db, UserManager, un: userNotification);
                        break;
                    case NotificationCategory.Role:
                        success = await NotificationHelper.RemoveNotificationAsync(db, UserManager, rn: await db.RoleNotifications.Include(rn => rn.NotificationAbout).Include(rn => rn.Role).FirstOrDefaultAsync(rn => rn.Id == notificationId));
                        break;
                }
            }

            if (newTicketLog)
                return RedirectToAction("Index", success ? new { ViewMessage = ViewMessage.DismissedNotification } : new { ViewMessage = ViewMessage.FailedToDismissNotification });

            return RedirectToAction("Index", success ? new { ViewMessage = ViewMessage.DeclinedRoleFromNotification } : new { ViewMessage = ViewMessage.FailedToDeclineRoleFromNotification });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
