using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        public async Task<ActionResult> _Partial_SocialMedia_Notifications(bool ticketsDay, bool ticketsWeek, bool ticketsMonth, bool ticketsTotal, bool timeDay, bool timeWeek, bool timeMonth, bool timeTotal)
        {
            List<SocialMediaNotificationViewModel> socialMediaNotifications = new List<SocialMediaNotificationViewModel>();
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            DateTime now = DateTime.Now;
            TicketState closedState = await db.TicketStates.FirstOrDefaultAsync(ts => ts.Name == "Closed");
            if (closedState == null)
                return null;
            double totalAmountOfTickets = await db.Tickets.CountAsync(t => t.TicketState.Id != closedState.Id);

            if (ticketsDay) // The percent of Tickets closed today, compared to Tickets opened today.
            {
                DateTime previousDay = now.AddDays(-1);
                double amountOfTicketsOpenedToday = await db.Tickets.Where(t => t.Created > previousDay).CountAsync();
                double amountOfTicketsClosedToday = await db.Tickets.Where(t => t.TicketState.Id == closedState.Id && t.LastMessage > previousDay).CountAsync();

                if ((int)amountOfTicketsOpenedToday != 0) // Avoid a divide by zero error.
                {
                    double percentageClosed = amountOfTicketsClosedToday / amountOfTicketsOpenedToday;
                    if (percentageClosed > 0)
                        socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                            $"We resolved {percentageClosed.ToString("P0")} of your issues today!"));
                }
                else
                {
                    if ((int)totalAmountOfTickets != 0) // Avoid a divide by zero error.
                    {
                        double percentageClosed = amountOfTicketsClosedToday / totalAmountOfTickets;
                        if(percentageClosed > 0)
                            socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                                $"We resolved {percentageClosed.ToString("P0")} extra, of your issues today!"));
                    }
                }
            }

            if (ticketsWeek) // The percent of Tickets closed this week, compared to Tickets opened this week.
            {
                DateTime previousWeek = now.AddDays(-7);
                double amountOfTicketsOpenedThisWeek = await db.Tickets.Where(t => t.Created > previousWeek).CountAsync();
                double amountOfTicketsClosedThisWeek = await db.Tickets.Where(t => t.TicketState.Id == closedState.Id && t.LastMessage > previousWeek).CountAsync();

                if ((int)amountOfTicketsOpenedThisWeek != 0) // Avoid a divide by zero error.
                {
                    double percentageClosed = amountOfTicketsClosedThisWeek/amountOfTicketsOpenedThisWeek;
                    if (percentageClosed > 0)
                        socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                            $"We resolved {percentageClosed.ToString("P0")} of your issues this week!"));
                }
                else
                {
                    if ((int)totalAmountOfTickets != 0) // Avoid a divide by zero error.
                    {
                        double percentageClosed = amountOfTicketsClosedThisWeek / totalAmountOfTickets;
                        if (percentageClosed > 0)
                            socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                                $"We resolved {percentageClosed.ToString("P0")} extra, of your issues this week!"));
                    }
                }
            }
            if (ticketsMonth) // The percent of Tickets closed this month, compared to Tickets opened this month.
            {
                DateTime previousMonth = now.AddMonths(-1);
                double amountOfTicketsOpenedThisMonth = await db.Tickets.Where(t => t.Created > previousMonth).CountAsync();
                double amountOfTicketsClosedThisMonth = await db.Tickets.Where(t => t.TicketState.Id == closedState.Id && t.LastMessage > previousMonth).CountAsync();

                if ((int)amountOfTicketsOpenedThisMonth != 0) // Avoid a divide by zero error.
                {
                    double percentageClosed = amountOfTicketsClosedThisMonth / amountOfTicketsOpenedThisMonth;
                    if (percentageClosed > 0)
                        socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                            $"We resolved {percentageClosed.ToString("P0")} of your issues this month!"));
                }
                else
                {
                    if ((int)totalAmountOfTickets != 0) // Avoid a divide by zero error.
                    {
                        double percentageClosed = amountOfTicketsClosedThisMonth / totalAmountOfTickets;
                        if (percentageClosed > 0)
                            socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                                $"We resolved {percentageClosed.ToString("P0")} extra, of your issues this month!"));
                    }
                }
            }
            if (ticketsTotal) // The percent of Tickets closed in total, compared to Tickets opened in total.
            {
                double amountOfTicketsClosed = await db.Tickets.CountAsync(t => t.TicketState.Id == closedState.Id);

                if ((int)totalAmountOfTickets != 0) // Avoid a divide by zero error.
                {
                    double percentageClosed = amountOfTicketsClosed / totalAmountOfTickets;
                    if (percentageClosed > 0)
                        socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                            $"So far we have resolved {percentageClosed.ToString("P0")} of all of your issues to date!"));
                }
            }

            if (timeDay) // The average time to respond to new Tickets today.
            {
                DateTime previousDay = now.AddDays(-1);
                List<Ticket> newTickets = await db.Tickets.Where(t => t.Created > previousDay).ToListAsync();
                int newTicketsRespondedTo = 0;
                double totalNewTicketResponseTimeInSeconds = 0;

                foreach (Ticket ticket in newTickets)
                {
                    if (await db.TicketLogs.AnyAsync(tl => tl.TicketId == ticket.Id)) // If there are any Logs for the Ticket
                    {
                        // Go through the Ticket Logs and find the first one that is from an Internal User, that will be the first response.
                        foreach (TicketLog ticketLog in (await db.TicketLogs.Where(tl => tl.TicketId == ticket.Id).Include(tl => tl.SubmittedByUser).OrderBy(tl => tl.TimeOfLog).ToListAsync()))
                        {
                            // If the Ticket Log was by an internal User, and an external message.
                            if (userManager.GetRoles(ticketLog.SubmittedByUser.Id).Contains(MyRoles.Internal) && !ticketLog.IsInternal)
                            {
                                // We have a response from an internal User to the new Ticket.
                                newTicketsRespondedTo++;
                                totalNewTicketResponseTimeInSeconds += (ticketLog.TimeOfLog - ticket.Created).TotalSeconds;
                            }
                        }
                    }
                }
                long ratioOfTicketsResponded = newTicketsRespondedTo/newTickets.Count;
                if (newTickets.Count != 0 && ratioOfTicketsResponded > 0)
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                        $"We got back to {ratioOfTicketsResponded.ToString("P0")} of all new Tickets today!"));

                if (newTicketsRespondedTo != 0)
                {
                    TimeSpan averageResponseTime = TimeSpan.FromSeconds((totalNewTicketResponseTimeInSeconds / newTicketsRespondedTo));
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                        $"For our responded to Tickets today, on average we got back to you within {averageResponseTime.Minutes} minute(s)!"));
                }
            }
            if (timeWeek) // The average time to respond to new Tickets this week.
            {
                DateTime previousWeek = now.AddDays(-7);
                List<Ticket> newTickets = await db.Tickets.Where(t => t.Created > previousWeek).ToListAsync();
                int newTicketsRespondedTo = 0;
                double totalNewTicketResponseTimeInSeconds = 0;

                foreach (Ticket ticket in newTickets)
                {
                    if (await db.TicketLogs.AnyAsync(tl => tl.TicketId == ticket.Id)) // If there are any Logs for the Ticket
                    {
                        // Go through the Ticket Logs and find the first one that is from an Internal User, that will be the first response.
                        foreach (TicketLog ticketLog in (await db.TicketLogs.Where(tl => tl.TicketId == ticket.Id).Include(tl => tl.SubmittedByUser).OrderBy(tl => tl.TimeOfLog).ToListAsync()))
                        {
                            // If the Ticket Log was by an internal User, and an external message.
                            if (userManager.GetRoles(ticketLog.SubmittedByUser.Id).Contains(MyRoles.Internal) && !ticketLog.IsInternal)
                            {
                                // We have a response from an internal User to the new Ticket.
                                newTicketsRespondedTo++;
                                totalNewTicketResponseTimeInSeconds += (ticketLog.TimeOfLog - ticket.Created).TotalSeconds;
                            }
                        }
                    }
                }
                long ratioOfTicketsResponded = newTicketsRespondedTo / newTickets.Count;
                if (newTickets.Count != 0 && ratioOfTicketsResponded > 0)
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                        $"We got back to {ratioOfTicketsResponded.ToString("P0")} of all new Tickets this week!"));

                if (newTicketsRespondedTo != 0)
                {
                    TimeSpan averageResponseTime = TimeSpan.FromSeconds((totalNewTicketResponseTimeInSeconds / newTicketsRespondedTo));
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                        $"For our responded to Tickets this week, on average we got back to you within {averageResponseTime.Minutes} minute(s)!"));
                }
            }
            if (timeMonth) // The average time to respond to new Tickets this month.
            {
                DateTime previousMonth = now.AddMonths(-1);
                List<Ticket> newTickets = await db.Tickets.Where(t => t.Created > previousMonth).ToListAsync();
                int newTicketsRespondedTo = 0;
                double totalNewTicketResponseTimeInSeconds = 0;

                foreach (Ticket ticket in newTickets)
                {
                    if (await db.TicketLogs.AnyAsync(tl => tl.TicketId == ticket.Id)) // If there are any Logs for the Ticket
                    {
                        // Go through the Ticket Logs and find the first one that is from an Internal User, that will be the first response.
                        foreach (TicketLog ticketLog in (await db.TicketLogs.Where(tl => tl.TicketId == ticket.Id).Include(tl => tl.SubmittedByUser).OrderBy(tl => tl.TimeOfLog).ToListAsync()))
                        {
                            // If the Ticket Log was by an internal User, and an external message.
                            if (userManager.GetRoles(ticketLog.SubmittedByUser.Id).Contains(MyRoles.Internal) && !ticketLog.IsInternal)
                            {
                                // We have a response from an internal User to the new Ticket.
                                newTicketsRespondedTo++;
                                totalNewTicketResponseTimeInSeconds += (ticketLog.TimeOfLog - ticket.Created).TotalSeconds;
                            }
                        }
                    }
                }
                long ratioOfTicketsResponded = newTicketsRespondedTo / newTickets.Count;
                if (newTickets.Count != 0 && ratioOfTicketsResponded > 0)
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                        $"We got back to {ratioOfTicketsResponded.ToString("P0")} of all new Tickets this month!"));

                if (newTicketsRespondedTo != 0)
                {
                    TimeSpan averageResponseTime = TimeSpan.FromSeconds((totalNewTicketResponseTimeInSeconds / newTicketsRespondedTo));
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                        $"For our responded to Tickets this month, on average we got back to you within {averageResponseTime.Minutes} minute(s)!"));
                }
            }
            if (timeTotal) // The average time to respond to new Tickets in total.
            {
                List<Ticket> newTickets = await db.Tickets.ToListAsync();
                int newTicketsRespondedTo = 0;
                double totalNewTicketResponseTimeInSeconds = 0;

                foreach (Ticket ticket in newTickets)
                {
                    if (await db.TicketLogs.AnyAsync(tl => tl.TicketId == ticket.Id)) // If there are any Logs for the Ticket
                    {
                        // Go through the Ticket Logs and find the first one that is from an Internal User, that will be the first response.
                        foreach (TicketLog ticketLog in (await db.TicketLogs.Where(tl => tl.TicketId == ticket.Id).Include(tl => tl.SubmittedByUser).OrderBy(tl => tl.TimeOfLog).ToListAsync()))
                        {
                            // If the Ticket Log was by an internal User, and an external message.
                            if (userManager.GetRoles(ticketLog.SubmittedByUser.Id).Contains(MyRoles.Internal) && !ticketLog.IsInternal)
                            {
                                // We have a response from an internal User to the new Ticket.
                                newTicketsRespondedTo++;
                                totalNewTicketResponseTimeInSeconds += (ticketLog.TimeOfLog - ticket.Created).TotalSeconds;
                            }
                        }
                    }
                }
                long ratioOfTicketsResponded = newTicketsRespondedTo / newTickets.Count;
                if (newTickets.Count != 0 && ratioOfTicketsResponded > 0)
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                        $"We got back to {ratioOfTicketsResponded.ToString("P0")} of all new Tickets so far!"));

                if (newTicketsRespondedTo != 0)
                {
                    TimeSpan averageResponseTime = TimeSpan.FromSeconds((totalNewTicketResponseTimeInSeconds / newTicketsRespondedTo));
                    socialMediaNotifications.Add(new SocialMediaNotificationViewModel(socialMediaNotifications.Count + 1,
                        $"For our responded to Tickets, on average we got back to you within {averageResponseTime.Minutes} minute(s)!"));
                }
            }

            return PartialView("_Partial_SocialMediaSuggestions", new SocialMediaNotificationsViewModel(socialMediaNotifications));
        }

        public async Task<ActionResult> _Partial_SocialMedia_Management_Notifications(bool userTicketsDay, bool userTicketsWeek, bool userTicketsMonth, bool userTicketsTotal,
                                                                                        bool userRepliesDay, bool userRepliesWeek, bool userRepliesMonth, bool userRepliesTotal)
        {
            // TODO: Finish this off!!!

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
