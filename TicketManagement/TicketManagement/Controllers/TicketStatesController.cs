using System.Data.Entity;
using System.Linq;
using System.Net;
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

        // GET: TicketStates
        public ActionResult Index()
        {
            return View(db.TicketStates.ToList());
        }

        // GET: TicketStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketState ticketState = db.TicketStates.Find(id);
            if (ticketState == null)
            {
                return HttpNotFound();
            }
            return View(ticketState);
        }

        // GET: TicketStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TicketStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Colour,LastUpdated")] TicketState ticketState)
        {
            if (ModelState.IsValid)
            {
                db.TicketStates.Add(ticketState);
                db.SaveChanges();
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketStateAdded });
            }

            return View(ticketState);
        }

        // GET: TicketStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketState ticketState = db.TicketStates.Find(id);
            if (ticketState == null)
            {
                return HttpNotFound();
            }
            return View(ticketState);
        }

        // POST: TicketStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Colour,LastUpdated")] TicketState ticketState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketStateUpdated });
            }
            return View(ticketState);
        }

        // GET: TicketStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketState ticketState = db.TicketStates.Find(id);
            if (ticketState == null)
            {
                return HttpNotFound();
            }
            return View(ticketState);
        }

        // POST: TicketStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketState ticketState = db.TicketStates.Find(id);
            db.TicketStates.Remove(ticketState);
            db.SaveChanges();
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
