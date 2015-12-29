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
    public class TicketCategoriesController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: TicketCategories
        public ActionResult Index()
        {
            var ticketCategories = db.TicketCategories.Include(t => t.Project);
            return View(ticketCategories.ToList());
        }

        // GET: TicketCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketCategory ticketCategory = db.TicketCategories.Find(id);
            if (ticketCategory == null)
            {
                return HttpNotFound();
            }
            return View(ticketCategory);
        }

        // GET: TicketCategories/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: TicketCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ProjectId,LastUpdated")] TicketCategory ticketCategory)
        {
            if (ModelState.IsValid)
            {
                db.TicketCategories.Add(ticketCategory);
                db.SaveChanges();
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketCategoryAdded });
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketCategory.ProjectId);
            return View(ticketCategory);
        }

        // GET: TicketCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketCategory ticketCategory = db.TicketCategories.Find(id);
            if (ticketCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketCategory.ProjectId);
            return View(ticketCategory);
        }

        // POST: TicketCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ProjectId,LastUpdated")] TicketCategory ticketCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketCategoryUpdated });
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketCategory.ProjectId);
            return View(ticketCategory);
        }

        // GET: TicketCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketCategory ticketCategory = db.TicketCategories.Find(id);
            if (ticketCategory == null)
            {
                return HttpNotFound();
            }
            return View(ticketCategory);
        }

        // POST: TicketCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketCategory ticketCategory = db.TicketCategories.Find(id);
            db.TicketCategories.Remove(ticketCategory);
            db.SaveChanges();
            return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketCategoryDeleted });
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
