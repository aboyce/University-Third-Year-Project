using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Models.Context;
using TicketManagement.Models.Management;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    public class NotificationController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        [ChildActionOnly]
        public ActionResult Notifications(NotificationViewModel vm)
        {
            vm.Notification = "Yippie Kaya";
            return PartialView(vm);
        }

        //// GET: Notification/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Notification/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,UserIdNotificationOn,NotificationType,NotificationMessage")] UserNotification userNotification)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.UserNotifications.Add(userNotification);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(userNotification);
        //}

        //// POST: Notification/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    UserNotification userNotification = await db.UserNotifications.FindAsync(id);
        //    db.UserNotifications.Remove(userNotification);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
