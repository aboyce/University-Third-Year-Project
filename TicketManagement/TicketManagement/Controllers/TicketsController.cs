using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;
using TicketManagement.ViewModels;
using File = TicketManagement.Models.Entities.File;

namespace TicketManagement.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Tickets
        public ActionResult Index()
        {
            TicketSort sortType; // Defaults to the first one.
            var tickets = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);

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
                        string id = User.Identity.GetUserId();
                        tickets = tickets.Where(t => t.UserAssignedToId == id);
                        break;
                        case TicketSort.All: default:
                        break;
                }
            }

            return View(tickets.ToList());
        }

        // GET: Tickets/Ticket
        public async Task<ActionResult> Ticket(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            

            TicketViewModel vm = new TicketViewModel
            {
                Ticket = await db.Tickets.FindAsync(id),
                TicketLogs = await db.TicketLogs.Where(tl => tl.TicketId == id).ToListAsync()
            };

            if (vm.Ticket == null)
                return HttpNotFound();

            return View(vm);
        }

        // POST: Tickets/NewTicketLogMessage
        [HttpPost]
        public async Task<ActionResult> NewTicketLogMessage(NewTicketLogViewModel vm)
        {
            string userId = User.Identity.GetUserId();

            TicketLog ticketLog = new TicketLog
            {
                Ticket = db.Tickets.FirstOrDefault(t => t.Id == vm.TicketId),
                TicketId = vm.TicketId,
                TicketLogType = TicketLogType.Message,
                SubmittedByUserId = userId,
                SubmittedByUser = db.Users.Find(userId),
                Data = vm.Data,
                IsInternal = vm.IsInternal,
                TimeOfLog = DateTime.Now
            };

            db.TicketLogs.Add(ticketLog);
            await db.SaveChangesAsync();

            return RedirectToAction("Ticket", new {id = vm.TicketId, ViewMessage = ViewMessage.TicketMessageAdded });
        }

        // POST: Tickets/NewTicketLogFlie
        [HttpPost]
        public async Task<ActionResult> NewTicketLogFile(NewTicketLogViewModel vm, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var file = new File
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Image,
                    ContentType = upload.ContentType,
                };

                using (BinaryReader reader = new BinaryReader(upload.InputStream))
                {
                    file.Content = reader.ReadBytes(upload.ContentLength);
                }

                db.Files.Add(file);
                await db.SaveChangesAsync();

                string userId = User.Identity.GetUserId();

                TicketLog ticketLog = new TicketLog
                {
                    Ticket = db.Tickets.FirstOrDefault(t => t.Id == vm.TicketId),
                    TicketId = vm.TicketId,
                    TicketLogType = TicketLogType.File,
                    SubmittedByUserId = userId,
                    SubmittedByUser = db.Users.Find(userId),
                    FileId = file.Id,
                    IsInternal = vm.IsInternal,
                    TimeOfLog = DateTime.Now
                };

                db.TicketLogs.Add(ticketLog);
                await db.SaveChangesAsync();

                return RedirectToAction("Ticket", new { id = vm.TicketId, ViewMessage = ViewMessage.TicketFileAdded });
            }

            return RedirectToAction("Ticket", new { id = vm.TicketId, ViewMessage = ViewMessage.TicketFileNotAdded });
        }


        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.OpenedById = new SelectList(db.Users, "Id", "Id");
            ViewBag.OrganisationAssignedToId = new SelectList(db.Organisations, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TeamAssignedToId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.TicketCategoryId = new SelectList(db.TicketCategories, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStateId = new SelectList(db.TicketStates, "Id", "Name");
            ViewBag.UserAssignedToId = new SelectList(db.Users, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,OpenedById,TicketPriorityId,TeamAssignedToId,OrganisationAssignedToId,TicketStateId,ProjectId,TicketCategoryId,Deadline,LastMessage,LastResponse,LastUpdated")] Ticket ticket, string deadlineString)
        {
            if (deadlineString.IsNullOrWhiteSpace())
                ModelState.AddModelError("Deadline", "The deadline is required");
            else
            {
                DateTime deadline;
                deadlineString += " 12:00:00";
                if (DateTime.TryParse(deadlineString, out deadline))
                    ticket.Deadline = deadline;
                else
                    ModelState.AddModelError("Deadline", "The deadline not in the correct format 'dd/mm/yyyy'");
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
                db.SaveChanges();
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

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
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

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,OpenedById,TicketPriorityId,TeamAssignedToId,OrganisationAssignedToId,TicketStateId,ProjectId,TicketCategoryId,Deadline,LastMessage,LastResponse,LastUpdated")] Ticket ticket, string deadlineString)
        {
            ticket.UserAssignedToId = Request.Form["UserAssignedToId"];

            if (deadlineString.IsNullOrWhiteSpace())
                ModelState.AddModelError("Deadline", "The deadline is required");
            else
            {
                DateTime deadline;
                if (!deadlineString.Contains(":")) // Assume the time hasn't been set
                    deadlineString += " 12:00:00";
                if (DateTime.TryParse(deadlineString, out deadline))
                    ticket.Deadline = deadline;
                else
                    ModelState.AddModelError("Deadline", "The deadline not in the correct format 'dd/mm/yyyy'");
            }

            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
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

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
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
