using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TicketManagement.Helpers;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Controllers
{
    //[Authorize(Roles = "TextMessage")] TODO: Add this back in.
    public class TextMessageController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Index
        public ActionResult Index()
        {
            ViewBag.Id = new SelectList(db.UserExtras, "ApplicationUserId", "FullName");

            return View();
        }

        // POST: Index
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string id, string message)
        {
            if (string.IsNullOrEmpty(id))
                ModelState.AddModelError("Id", "Please ensure there is a Recipient selected.");

            if (string.IsNullOrEmpty(message))
                ModelState.AddModelError("Message", "Please ensure there is a Message body.");

            int? maxLength = Helpers.Configuration.GetTextMessageMaxLength();

            if (maxLength == null)
                ModelState.AddModelError(string.Empty, "Problem reading max text message length from the config, please see a system administrator.");

            if (message != null && message.Length > maxLength)
                ModelState.AddModelError("Message", $"Please ensure the message is less than {maxLength} characters.");

            if (ModelState.IsValid)
            {
                TextMessageManager txtmsg = new TextMessageManager();

                var number = db.Users.Where(u => u.Id == id).Select(p => p.PhoneNumber);

                //db.Organisations.Add(organisation);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.UserExtras, "ApplicationUserId", "FullName");
            return View();
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
