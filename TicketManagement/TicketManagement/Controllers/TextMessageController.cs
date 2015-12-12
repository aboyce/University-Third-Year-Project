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
using TicketManagement.Models.Management;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = "TextMessage")]
    public class TextMessageController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Index
        public async Task<ActionResult> Index()
        {
            var textMessages = db.TextMessages.Select(tm => tm);
            return View(await textMessages.ToListAsync());
        }

        // GET: Send
        public ActionResult Send()
        {
            ViewBag.Id = new SelectList(db.UserExtras, "ApplicationUserId", "FullName");

            return View();
        }

        // POST: Send
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(string id, string message)
        {
            if (string.IsNullOrEmpty(id))
                ModelState.AddModelError("Id", "Please ensure there is a Recipient selected.");

            if (string.IsNullOrEmpty(message))
                ModelState.AddModelError("Message", "Please ensure there is a Message body.");

            int? maxLength = Helpers.ConfigurationHelper.GetTextMessageMaxLength();

            if (maxLength == null)
                ModelState.AddModelError(string.Empty, "Problem reading max text message length from the config, please see a system administrator.");

            if (message != null && message.Length > maxLength)
                ModelState.AddModelError("Message", $"Please ensure the message is less than {maxLength} characters.");

            if (ModelState.IsValid)
            {
                TextMessageHelper txtManager = new TextMessageHelper();
                TextMessage txt;
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == id);

                string number = user?.PhoneNumber;

                txt = new TextMessage(id, user, number, message);

                string result;// = txtmsg.SendTextMessage(txt);

                result = null;

                if (result != null)
                {
                    ViewBag.ErrorMessage = result;
                    ViewBag.Id = new SelectList(db.UserExtras, "ApplicationUserId", "FullName");
                    ViewBag.TextResult = TextResult.SendFailure;
                    return View();
                }
                else
                {
                    db.TextMessages.Add(txt);
                    db.SaveChangesAsync();

                    ViewBag.Id = new SelectList(db.UserExtras, "ApplicationUserId", "FullName");
                    ViewBag.TextResult = TextResult.SendSuccess;
                    ViewBag.RemainingBalance = txtManager.CheckBalance();
                    return View();
                }
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
