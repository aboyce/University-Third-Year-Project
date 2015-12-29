using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Helpers;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;
using TicketManagement.Properties;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = "Administrator, TextMessage")]
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
            ViewBag.Id = new SelectList(db.Users, "Id", "FullName");

            return View();
        }

        // POST: Send
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
                TextMessage txt = new TextMessage(id, user, user?.PhoneNumber, message);

                string result = await txtManager.SendTextMessageAsync(txt);

                if (result != null)
                {
                    ViewBag.ErrorMessage = result;
                    ViewBag.Id = new SelectList(db.Users, "Id", "FullName");
                    ViewBag.TextResult = TextResult.SendFailure;
                    return View();
                }
                else
                {
                    db.TextMessages.Add(txt);
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
