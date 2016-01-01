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
    public class TeamsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Organisation);
            return View(teams.ToList());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Team team = await db.Teams.FindAsync(id);

            if (team == null)
                return HttpNotFound();
            
            return View(team);
        }

        public ActionResult Create()
        {
            ViewBag.OrganisationId = new SelectList(db.Organisations, "Id", "Name");

            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,OrganisationId,LastUpdated")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TeamAdded });
            }

            ViewBag.OrganisationId = new SelectList(db.Organisations, "Id", "Name", team.OrganisationId);

            return View(team);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Team team = await db.Teams.FindAsync(id);

            if (team == null)
                return HttpNotFound();
            
            ViewBag.OrganisationId = new SelectList(db.Organisations, "Id", "Name", team.OrganisationId);

            return View(team);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,OrganisationId,LastUpdated")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TeamUpdated });
            }

            ViewBag.OrganisationId = new SelectList(db.Organisations, "Id", "Name", team.OrganisationId);

            return View(team);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Team team = await db.Teams.FindAsync(id);

            if (team == null)
                return HttpNotFound();
            
            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Team team = await db.Teams.FindAsync(id);

            db.Teams.Remove(team);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ViewMessage = ViewMessage.TeamDeleted });
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
