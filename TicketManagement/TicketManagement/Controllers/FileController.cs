using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Controllers
{
    public class FileController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: File
        public async Task<ActionResult> Index(int id)
        {
            var fileToRetrieve = await db.Files.FindAsync(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}
