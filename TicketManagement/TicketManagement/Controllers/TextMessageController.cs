using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Properties;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = "Administrator, TextMessage")]
    public class TextMessageController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> _Partial_SentMessages()
        {
            return PartialView(await db.TextMessagesSent.Select(tms => tms).Include(tms => tms.UserTo).ToListAsync());
        }

        public async Task<ActionResult> _Partial_RecievedMessages()
        {
            return PartialView(await db.TextMessagesReceived.Select(tmr => tmr).Include(tmr => tmr.UserFrom).ToListAsync());
        }

        public ActionResult Send()
        {
            ViewBag.Id = new SelectList(db.Users, "Id", "FullName");

            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Send(string id, string message)
        {
            if (string.IsNullOrEmpty(id))
                ModelState.AddModelError("Id", Resources.TextMessageController_Send_RecipientSelectionRequired);

            if (string.IsNullOrEmpty(message))
                ModelState.AddModelError("Message", Resources.TextMessageController_Send_MessageBodyRequired);

            int? maxLength = await ConfigurationHelper.GetTextMessageMaxLengthAsync();

            if (maxLength == null)
                ModelState.AddModelError(string.Empty, Resources.TextMessageController_Send_ErrorMaxLengthFromConfig);

            if (message != null && message.Length > maxLength)
                ModelState.AddModelError("Message", $"Please ensure the message is less than {maxLength} characters.");

            if (ModelState.IsValid)
            {
                TextMessageHelper txtManager = new TextMessageHelper();
                User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

                SentTextMessage txt = await txtManager.SendTextMessageAsync(id, user, user?.PhoneNumber, message);

                if (txt.Success == false)
                {
                    ViewBag.ErrorMessage = txt.ErrorMessage;
                    ViewBag.Id = new SelectList(db.Users, "Id", "FullName");
                    ViewBag.TextResult = TextResult.SendFailure;
                    return View();
                }
                else
                {
                    db.TextMessagesSent.Add(txt);
                    await db.SaveChangesAsync();

                    ViewBag.Id = new SelectList(db.Users, "Id", "FullName");
                    ViewBag.TextResult = TextResult.SendSuccess;
                    ViewBag.RemainingBalance = await txtManager.CheckBalanceAsync();
                    return View();
                }
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendNotifications(string id)
        {
            if (string.IsNullOrEmpty(id))
                ModelState.AddModelError("Id", Resources.TextMessageController_Send_RecipientSelectionRequired);

            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (string.IsNullOrEmpty(user?.PhoneNumber))
            {
                ViewBag.TextResult = TextResult.SendFailure;
                return RedirectToAction("Send");
            }

            TextMessageProtocolHelper txtHelper = new TextMessageProtocolHelper(db);

            if (await txtHelper.ProcessGetNotifications(user.PhoneNumber))
                ViewBag.TextResult = TextResult.SendSuccess;
            else
                ViewBag.TextResult = TextResult.SendFailure;

            return RedirectToAction("Send");
        }

        [HttpPost]
        public async Task<ActionResult> SendHelp(string id)
        {
            if (string.IsNullOrEmpty(id))
                ModelState.AddModelError("Id", Resources.TextMessageController_Send_RecipientSelectionRequired);

            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (string.IsNullOrEmpty(user?.PhoneNumber))
            {
                ViewBag.TextResult = TextResult.SendFailure;
                return RedirectToAction("Send");
            }

            TextMessageProtocolHelper txtHelper = new TextMessageProtocolHelper(db);

            if (await txtHelper.ProcessHelpText(user.PhoneNumber))
                ViewBag.TextResult = TextResult.SendSuccess;
            else
                ViewBag.TextResult = TextResult.SendFailure;

            return RedirectToAction("Send");
        }

        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        public async Task<ActionResult> Receive()
        {
            string xmlString = "";

            if (Request.InputStream != null)
            {
                StreamReader stream = new StreamReader(Request.InputStream);
                xmlString = HttpUtility.UrlDecode(stream.ReadToEnd()); ;
            }

            TextMessageHelper txtHelper = new TextMessageHelper();
            ReceivedTextMessage txt = txtHelper.ReceiveTextMessage(xmlString);

            if (txt == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            txt = await txtHelper.ProcessTextMessage(txt, db);

            db.TextMessagesReceived.Add(txt);
            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [ValidateInput(false)]
        [AllowAnonymous]
        public async Task<ActionResult> ReceiveDeliveryReceipts(string msg_id, string status, string detail, string to)
        {
            if (string.IsNullOrEmpty(msg_id) || string.IsNullOrEmpty(status) || string.IsNullOrEmpty(detail))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                SentTextMessage txt = await db.TextMessagesSent.Include(t => t.UserTo).FirstOrDefaultAsync(t => t.ClockworkId == msg_id);

                txt.DeliveryStatus = status;
                txt.DeliveryDetail = detail;

                db.Entry(txt).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            finally 
            { } // All they want back is a 200 OK, so if we have problems but they sent valid information, that is all we can really do.

            return new HttpStatusCodeResult(HttpStatusCode.OK);
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
