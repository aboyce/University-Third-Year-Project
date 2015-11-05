using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.Controllers
{
    public class WelcomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }
    }
}
