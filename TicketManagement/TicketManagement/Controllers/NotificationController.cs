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

        public ActionResult Index(NotificationViewModel vm)
        {
            User user = UserManager.FindById(User.Identity.GetUserId());

            vm.RoleNotifications = NotificationHelper.GetRoleNotificationsForUser(db, user.Id, UserManager.GetRoles(user.Id));
            vm.UserNotifications = NotificationHelper.GetUserNotificationsForUser(db, user.Id);

            return View(vm);
        }

        [ChildActionOnly]
        public ActionResult _Partial_Notifications(NotificationViewModel vm)
        {
            User user = UserManager.FindById(User.Identity.GetUserId());

            vm.RoleNotifications = NotificationHelper.GetRoleNotificationsForUser(db, user.Id, UserManager.GetRoles(user.Id));
            vm.UserNotifications = NotificationHelper.GetUserNotificationsForUser(db, user.Id);

            return PartialView(vm);
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

        public async Task<ActionResult> DeclineNotification(NotificationViewModel vm, NotificationCategory notificationCategory, int notificationId)
        {
            bool success = false;

            if (notificationId > 0)
            {
                if (notificationCategory == NotificationCategory.User)
                {
                    success = await NotificationHelper.DeclineNotificationAsync(db, UserManager,
                                un: await db.UserNotifications.Include(un => un.NotificationAbout).FirstOrDefaultAsync(un => un.Id == notificationId)); 
                }
                else if (notificationCategory == NotificationCategory.Role)
                {
                    success = await NotificationHelper.DeclineNotificationAsync(db, UserManager,
                                rn: await db.RoleNotifications.Include(rn => rn.NotificationAbout).Include(rn => rn.Role).FirstOrDefaultAsync(rn => rn.Id == notificationId));
                }
            }

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
