using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TicketStatesController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.TicketStates.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketState ticketState = await db.TicketStates.FindAsync(id);

            if (ticketState == null)
                return HttpNotFound();
            
            return View(ticketState);
        }

        public ActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Colour,LastUpdated")] TicketState ticketState)
        {
            if (ModelState.IsValid)
            {
                db.TicketStates.Add(ticketState);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketStateAdded });
            }

            return View(ticketState);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketState ticketState = await db.TicketStates.FindAsync(id);

            if (ticketState == null)
                return HttpNotFound();
            
            return View(ticketState);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Colour,LastUpdated")] TicketState ticketState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketState).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketStateUpdated });
            }

            return View(ticketState);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketState ticketState = await db.TicketStates.FindAsync(id);

            if (ticketState == null)
                return HttpNotFound();
            
            return View(ticketState);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TicketState ticketState = await db.TicketStates.FindAsync(id);

            db.TicketStates.Remove(ticketState);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketStateDeleted });
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
