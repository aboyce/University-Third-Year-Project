using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Administrator)]
    public class TicketPrioritiesController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.TicketPriorities.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketPriority ticketPriority = await db.TicketPriorities.FindAsync(id);

            if (ticketPriority == null)
                return HttpNotFound();
            
            return View(ticketPriority);
        }

        public ActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Colour,LastUpdated")] TicketPriority ticketPriority)
        {
            if (ModelState.IsValid)
            {
                db.TicketPriorities.Add(ticketPriority);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketPriorityAdded });
            }

            return View(ticketPriority);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketPriority ticketPriority = await db.TicketPriorities.FindAsync(id);

            if (ticketPriority == null)
                return HttpNotFound();
            
            return View(ticketPriority);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Colour,LastUpdated")] TicketPriority ticketPriority)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketPriority).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketPriorityUpdated });
            }
            return View(ticketPriority);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketPriority ticketPriority = await db.TicketPriorities.FindAsync(id);

            if (ticketPriority == null)
                return HttpNotFound();
            
            return View(ticketPriority);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TicketPriority ticketPriority = await db.TicketPriorities.FindAsync(id);

            db.TicketPriorities.Remove(ticketPriority);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketPriorityDeleted });
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
