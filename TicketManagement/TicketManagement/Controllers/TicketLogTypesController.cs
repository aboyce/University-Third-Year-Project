using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TicketLogTypesController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: TicketLogTypes
        public ActionResult Index()
        {
            return View(db.TicketLogTypes.ToList());
        }

        // GET: TicketLogTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketLogType ticketLogType = db.TicketLogTypes.Find(id);
            if (ticketLogType == null)
            {
                return HttpNotFound();
            }
            return View(ticketLogType);
        }

        // GET: TicketLogTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TicketLogTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LastUpdated")] TicketLogType ticketLogType)
        {
            if (ModelState.IsValid)
            {
                db.TicketLogTypes.Add(ticketLogType);
                db.SaveChanges();
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketLogTypeAdded });
            }

            return View(ticketLogType);
        }

        // GET: TicketLogTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketLogType ticketLogType = db.TicketLogTypes.Find(id);
            if (ticketLogType == null)
            {
                return HttpNotFound();
            }
            return View(ticketLogType);
        }

        // POST: TicketLogTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LastUpdated")] TicketLogType ticketLogType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketLogType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketLogTypeUpdated });
            }
            return View(ticketLogType);
        }

        // GET: TicketLogTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketLogType ticketLogType = db.TicketLogTypes.Find(id);
            if (ticketLogType == null)
            {
                return HttpNotFound();
            }
            return View(ticketLogType);
        }

        // POST: TicketLogTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketLogType ticketLogType = db.TicketLogTypes.Find(id);
            db.TicketLogTypes.Remove(ticketLogType);
            db.SaveChanges();
            return RedirectToAction("Index", new { ViewMessage = ViewMessage.TicketLogTypeDeleted });
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
