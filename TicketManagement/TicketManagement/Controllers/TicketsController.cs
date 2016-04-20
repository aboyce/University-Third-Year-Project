using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Properties;
using TicketManagement.ViewModels;
using File = TicketManagement.Models.Entities.File;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Approved)]
    public class TicketsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public async Task<ActionResult> Index()
        {
            string id = User.Identity.GetUserId();
            TicketSort sortType; // Defaults to the first one.
            var tickets = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);

            if (!User.IsInRole(MyRoles.Internal)) // If the user is not internal than they should only be able to see thier tickets.
                tickets = tickets.Where(t => t.OpenedById == id);

            if (Enum.TryParse(Request.QueryString["sort"], out sortType)) // Get the sort type from the tabs on Index. 
            {
                switch (sortType)
                {
                    case TicketSort.Open:
                        tickets = tickets.Where(t => t.TicketState.Name == "Open");
                        break;
                    case TicketSort.Closed:
                        tickets = tickets.Where(t => t.TicketState.Name == "Closed");
                        break;
                    case TicketSort.Unanswered:
                        tickets = tickets.Where(t => t.TicketState.Name == "Awaiting Response");
                        break;
                    case TicketSort.PendingApproval:
                        tickets = tickets.Where(t => t.TicketState.Name == "Pending Approval");
                        break;
                    case TicketSort.Mine:
                        tickets = tickets.Where(t => t.UserAssignedToId == id);
                        break;
                        //case TicketSort.All:
                        //default:
                        //    break;
                }
            }

            return View(await tickets.ToListAsync());
        }

        public async Task<ActionResult> Ticket(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ticketLogs = db.TicketLogs.Where(tl => tl.TicketId == id);

            if (!User.IsInRole(MyRoles.Internal))
                ticketLogs = ticketLogs.Where(tl => tl.IsInternal == false);

            TicketViewModel vm = new TicketViewModel
            {
                Ticket = await db.Tickets.FindAsync(id),
                TicketLogs = await ticketLogs.ToListAsync()
            };

            if (vm.Ticket == null)
                return HttpNotFound();

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> NewTicketLog(NewTicketLogViewModel vm, HttpPostedFileBase upload)
        {
            File file = null;

            TicketLogType type = User.IsInRole(MyRoles.Internal) ? TicketLogType.MessageFromInternalUser : TicketLogType.MessageFromExternalUser;

            if (upload != null && upload.ContentLength > 0) // We have a file to handle
            {
                file = new File
                {
                    FileName = Path.GetFileName(upload.FileName),
                    ContentType = upload.ContentType,
                };

                if (upload.ContentType.Contains("image"))
                    file.FileType = FileType.Image;
                else if (upload.ContentType.Contains("text"))
                    file.FileType = FileType.Text;
                else if (upload.ContentType.Contains("pdf"))
                    file.FileType = FileType.PDF;
                else
                    ModelState.AddModelError("", Resources.TicketsController_NewTicketLogFile_UnsupportedFileTypes);

                using (BinaryReader reader = new BinaryReader(upload.InputStream))
                    file.Content = reader.ReadBytes(upload.ContentLength);

                db.Files.Add(file);
                await db.SaveChangesAsync();
            }

            if (vm.SendSms) // Assume if SendSms is true that the logged in User is Internal (as external Users shouldn't be able to configure sending an SMS).
            {
                string externalUserId = await db.Tickets.Where(t => t.Id == vm.TicketId).Select(t => t.OpenedById).FirstOrDefaultAsync();
                User externalUser = await db.Users.FirstOrDefaultAsync(u => u.Id == externalUserId);
                if (externalUser != null)
                {
                    SentTextMessage text = await new TextMessageHelper().SendTextMessageAsync(externalUser.Id, externalUser, externalUser.PhoneNumber,
                        $"An update has been made to one of your Tickets by {User.Identity.Name}: {vm.Message}");

                    if (text != null)
                    {
                        db.TextMessagesSent.Add(text);
                        await db.SaveChangesAsync();
                    }
                }
            }

            if (await TicketLogHelper.NewTicketLogAsync(User.Identity.GetUserId(), vm.TicketId, type, vm.IsInternal, vm.CloseOnReply, db, vm.Message, file))
                return RedirectToAction("Ticket", new { id = vm.TicketId, ViewMessage = ViewMessage.TicketMessageAdded });
            else
                return RedirectToAction("Ticket", new { id = vm.TicketId, ViewMessage = ViewMessage.TicketMessageNotAdded });
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Ticket ticket = await db.Tickets.FindAsync(id);

            if (ticket == null)
                return HttpNotFound();

            return View(ticket);
        }

        public ActionResult Create(string title, string description)
        {
            ViewBag.OpenedById = new SelectList(db.Users, "Id", "Id");
            ViewBag.OrganisationAssignedToId = new SelectList(db.Organisations, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.TicketCategoryId = new SelectList(db.TicketCategories, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStateId = new SelectList(db.TicketStates, "Id", "Name");
            ViewBag.UserAssignedToId = new SelectList(db.Users, "Id", "Id");

            if(!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(description))
                return View(new Ticket() { Title = title, Description = description });

            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,OpenedById,TicketPriorityId,TeamAssignedToId,OrganisationAssignedToId,TicketStateId,ProjectId,TicketCategoryId,Deadline,LastMessage,LastResponse,LastUpdated")] Ticket ticket, string deadlineString)
        {
            if (deadlineString.IsNullOrWhiteSpace())
                ModelState.AddModelError("Deadline", Resources.TicketsController_Create_DeadlineRequired);
            else
            {
                DateTime deadline;
                deadlineString += " 12:00:00";
                if (DateTime.TryParse(deadlineString, out deadline))
                    ticket.Deadline = deadline;
                else
                    ModelState.AddModelError("Deadline", Resources.TicketsController_Edit_DeadlineFormat);
            }

            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var applicationUser = db.Users.FirstOrDefault(u => u.Id == userId);
                if (applicationUser != null)
                {
                    ticket.OpenedBy = applicationUser;
                    ticket.OpenedById = applicationUser.Id;
                }

                db.Tickets.Add(ticket);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.OpenedById = new SelectList(db.Users, "Id", "Id", ticket.OpenedById);
            ViewBag.OrganisationAssignedToId = new SelectList(db.Organisations, "Id", "Name", ticket.OrganisationAssignedToId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name", ticket.TeamAssignedToId);
            ViewBag.TicketCategoryId = new SelectList(db.TicketCategories, "Id", "Name", ticket.TicketCategoryId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStateId = new SelectList(db.TicketStates, "Id", "Name", ticket.TicketStateId);
            ViewBag.UserAssignedToId = new SelectList(db.Users, "Id", "Id", ticket.UserAssignedToId);

            return View(ticket);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Ticket ticket = db.Tickets.Find(id);

            if (ticket == null)
                return HttpNotFound();

            ViewBag.OrganisationAssignedToId = new SelectList(db.Organisations, "Id", "Name", ticket.OrganisationAssignedToId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name", ticket.TeamAssignedToId);
            ViewBag.TicketCategoryId = new SelectList(db.TicketCategories, "Id", "Name", ticket.TicketCategoryId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStateId = new SelectList(db.TicketStates, "Id", "Name", ticket.TicketStateId);
            ViewBag.UserAssignedToId = new SelectList(db.Users, "Id", "FullName", ticket.UserAssignedToId);

            return View(ticket);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,OpenedById,TicketPriorityId,TeamAssignedToId,OrganisationAssignedToId,TicketStateId,ProjectId,TicketCategoryId,Deadline,LastMessage,LastResponse,LastUpdated")] Ticket ticket, string deadlineString)
        {
            ticket.UserAssignedToId = Request.Form["UserAssignedToId"];

            if (User.IsInRole(MyRoles.Internal))
            {
                if (deadlineString.IsNullOrWhiteSpace())
                    ModelState.AddModelError("Deadline", Resources.TicketsController_Create_DeadlineRequired);
                else
                {
                    DateTime deadline;
                    if (!deadlineString.Contains(":")) // Assume the time hasn't been set
                        deadlineString += " 12:00:00";
                    if (DateTime.TryParse(deadlineString, out deadline))
                        ticket.Deadline = deadline;
                    else
                        ModelState.AddModelError("Deadline", Resources.TicketsController_Edit_DeadlineFormat);
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OrganisationAssignedToId = new SelectList(db.Organisations, "Id", "Name", ticket.OrganisationAssignedToId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name", ticket.TeamAssignedToId);
            ViewBag.TicketCategoryId = new SelectList(db.TicketCategories, "Id", "Name", ticket.TicketCategoryId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStateId = new SelectList(db.TicketStates, "Id", "Name", ticket.TicketStateId);
            ViewBag.UserAssignedToId = new SelectList(db.Users, "Id", "FullName", ticket.UserAssignedToId);

            return View(ticket);
        }

        [Authorize(Roles=MyRoles.Administrator)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Ticket ticket = await db.Tickets.FindAsync(id);

            if (ticket == null)
                return HttpNotFound();

            return View(ticket);
        }

        [Authorize(Roles = MyRoles.Administrator)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);

            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
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
