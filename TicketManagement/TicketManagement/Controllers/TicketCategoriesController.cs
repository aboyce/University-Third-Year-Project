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
    public class TicketCategoriesController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public async Task<ActionResult> Index()
        {
            var ticketCategories = db.TicketCategories.Include(t => t.Project);
            return View(await ticketCategories.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketCategory ticketCategory = await db.TicketCategories.FindAsync(id);

            if (ticketCategory == null)
                return HttpNotFound();
            
            return View(ticketCategory);
        }

        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");

            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,ProjectId,LastUpdated")] TicketCategory ticketCategory)
        {
            if (ModelState.IsValid)
            {
                db.TicketCategories.Add(ticketCategory);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketCategoryAdded });
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketCategory.ProjectId);

            return View(ticketCategory);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketCategory ticketCategory = await db.TicketCategories.FindAsync(id);

            if (ticketCategory == null)
                return HttpNotFound();
            
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketCategory.ProjectId);

            return View(ticketCategory);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ProjectId,LastUpdated")] TicketCategory ticketCategory)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(ticketCategory).State = EntityState.Modified;
                db.MarkAsModified(ticketCategory);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketCategoryUpdated });
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticketCategory.ProjectId);

            return View(ticketCategory);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            TicketCategory ticketCategory = await db.TicketCategories.FindAsync(id);

            if (ticketCategory == null)
                return HttpNotFound();
            
            return View(ticketCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TicketCategory ticketCategory = await db.TicketCategories.FindAsync(id);

            db.TicketCategories.Remove(ticketCategory);
            await db.SaveChangesAsync();

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
