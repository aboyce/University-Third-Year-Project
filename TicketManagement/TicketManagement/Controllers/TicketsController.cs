using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.OpenedBy).Include(t => t.OrganisationAssignedTo).Include(t => t.Project).Include(t => t.TeamAssignedTo).Include(t => t.TicketCategory).Include(t => t.TicketPriority).Include(t => t.TicketState);
            return View(tickets.ToList());
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
        public ActionResult Create([Bind(Include = "Id,Title,Description,OpenedById,TicketPriorityId,TeamAssignedToId,OrganisationAssignedToId,TicketStateId,ProjectId,TicketCategoryId,Deadline,LastMessage,LastResponse,LastUpdated")] Ticket ticket)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                var applicationUser = db.Users.FirstOrDefault(u => u.Id == User.Identity.GetUserId());
                if (applicationUser != null)
                {
                    //ticket.OpenedBy = applicationUser;
                    //ticket.OpenedById = applicationUser.Id;
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

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,OpenedById,TicketPriorityId,TeamAssignedToId,OrganisationAssignedToId,TicketStateId,ProjectId,TicketCategoryId,Deadline,LastMessage,LastResponse,LastUpdated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
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
