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
    [Authorize(Roles = MyRoles.Administrator)]
    public class ProjectsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public async Task<ActionResult> Index()
        {
            var projects = db.Projects.Include(p => p.Organisation).Include(p => p.TeamAssignedTo);
            return View(await projects.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Project project = await db.Projects.FindAsync(id);

            if (project == null)
                return HttpNotFound();
            
            return View(project);
        }

        public ActionResult Create()
        {
            ViewBag.OrganisationId = new SelectList(db.Organisations, "Id", "Name");
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name");

            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,OrganisationId,TeamAssignedToId,LastUpdated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.ProjectAdded });
            }

            ViewBag.OrganisationId = new SelectList(db.Organisations, "Id", "Name", project.OrganisationId);
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name", project.TeamAssignedToId);

            return View(project);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Project project = await db.Projects.FindAsync(id);

            if (project == null)
                return HttpNotFound();
            
            ViewBag.OrganisationId = new SelectList(db.Organisations, "Id", "Name", project.OrganisationId);
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name", project.TeamAssignedToId);

            return View(project);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,OrganisationId,TeamAssignedToId,LastUpdated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.ProjectUpdated });
            }

            ViewBag.OrganisationId = new SelectList(db.Organisations, "Id", "Name", project.OrganisationId);
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name", project.TeamAssignedToId);

            return View(project);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Project project = await db.Projects.FindAsync(id);

            if (project == null)
                return HttpNotFound();
            
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Project project = await db.Projects.FindAsync(id);

            db.Projects.Remove(project);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ViewMessage = ViewMessage.ProjectDeleted });
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
