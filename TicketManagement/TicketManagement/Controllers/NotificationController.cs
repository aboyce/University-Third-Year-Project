using System.Data.Entity;
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

        public async Task<ActionResult> _Partial_SocialMedia_Notifications(bool ticketsDay, bool ticketsWeek, bool ticketsMonth, int? ticketsDayValue, int? ticketsWeekValue, int? ticketsMonthValue)
        {

            if (ticketsDay && ticketsDayValue != null)
            {
                // TODO: Get the tickets opened today
                // TODO: Get the tickets closed today
                // TODO: Work out the percent that have been closed
                // TODO: Add the details to a List of ViewModels (in Notification ViewModels)

                // TODO: Display the list as a table like the others

                // TODO: Add more options; 
                // TODO:    - ??? Think!
            }
            if (ticketsWeek && ticketsWeekValue != null)
            {

            }
            if (ticketsMonth && ticketsMonthValue != null)
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
