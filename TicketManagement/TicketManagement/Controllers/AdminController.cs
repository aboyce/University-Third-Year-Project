using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [Authorize(Roles=MyRoles.Administrator)]
    public class AdminController : Controller
    {
        #region Properties

        private ApplicationContext db = new ApplicationContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public AdminController()
        {
        }
        public AdminController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            Dictionary<char, int> totals = new Dictionary<char, int>
            {
                {'o', await db.Organisations.Select(o => o.Id).CountAsync()},
                {'u', await db.Users.Select(u => u.Id).CountAsync()},
                {'p', await db.Projects.Select(p => p.Id).CountAsync()},
                {'m', await db.Teams.Select(m => m.Id).CountAsync()},
                {'t', await db.Tickets.Select(t => t.Id).CountAsync()},
                {'g', await db.TicketCategories.Select(g => g.Id).CountAsync()},
                {'l', await db.TicketLogs.Select(l => l.Id).CountAsync()},
                {'i', await db.TicketPriorities.Select(i => i.Id).CountAsync()},
                {'e', await db.TicketStates.Select(e => e.Id).CountAsync()},
            };

            TicketConfiguration conf = await ConfigurationHelper.GetTicketConfigurationAsync();

            if (conf == null) return View(totals);

            totals.Add('1', (int)conf.TimeSpanGreen.TotalHours);
            totals.Add('2', (int)conf.TimeSpanAmber.TotalHours);
            totals.Add('3', (int)conf.TimeSpanRed.TotalHours);

            return View(totals);
        }

        public ActionResult Users()
        {
            return View(db.Users.Select(u => u).ToList().ToDictionary(user => user, user => UserManager.GetRoles(user.Id)));
        }

        public async Task<ActionResult> UserEdit(string id)
        {
            if (id == null)
                return View("Error", new ErrorViewModel {Type = ErrorType.Error, Message = "'Admin/UserEdit' has been visited from the wrong location, please ensure you are an admin and try again." });

            User user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return View("Error", new ErrorViewModel { Type = ErrorType.Error, Message = "Could not find user, please try again." });

            ViewBag.Teams = new SelectList(db.Teams, "Id", "Name", user.TeamId);
            ViewBag.RolesToAdd = new SelectList(db.Roles, "Id", "Name", user.Roles);
            ViewBag.RolesToRemove = new SelectList(db.Roles, "Id", "Name", user.Roles);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit([Bind(Include = "Id,FirstName,LastName,UserName,IsArchived,TeamId,IsTeamLeader,Created,LastUpdated,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (ModelState.IsValid)
            {
                int teamId = 0;
                Team team = null;

                if (int.TryParse(Request.Form["Teams"], out teamId))
                    team = await db.Teams.FirstOrDefaultAsync(t => t.Id == teamId);

                if (team != null)
                {
                    user.Team = team;
                    user.TeamId = team.Id;
                }
                
                user.PhoneNumber = await PhoneNumberHelper.FormatPhoneNumberForClockworkAsync(user.PhoneNumber);
                user.LastUpdated = DateTime.Now;

                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Users", "Admin", new { ViewMessage = ViewMessage.ProfileUpdated });
            }

            ViewBag.RolesToAdd = new SelectList(db.Roles, "Id", "Name", user.Roles);
            ViewBag.RolesToRemove = new SelectList(db.Roles, "Id", "Name", user.Roles);
            ViewBag.Teams = new SelectList(db.Teams, "Id", "Name", user.TeamId);

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> PopulateData()
        {
            DataPopulationHelper dataPopulationHelper = new DataPopulationHelper();

            if (await dataPopulationHelper.PopulateDemoDataAsync(db, UserManager))
                return RedirectToAction("Index", new {ViewMessage = ViewMessage.DataPopulated });
            else
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.DataNotPopulated });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(AddRemoveRoleViewmodel vm)
        {
            string roleId = Request.Form["RolesToAdd"];
            string roleName = await db.Roles.Where(r => r.Id == roleId).Select(rn => rn.Name).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(vm.UserId) || string.IsNullOrEmpty(roleId))
                return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.RoleNotAdded });

            if (roleName != MyRoles.Approved && roleName != MyRoles.Internal) // If it is a significant role, check they are internal.
                if (!await UserManager.IsInRoleAsync(vm.UserId, MyRoles.Internal))
                    return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.NotInternal });

            if (await UserManager.IsInRoleAsync(vm.UserId, roleName))
                return RedirectToAction("UserEdit", new {id = vm.UserId, ViewMessage = ViewMessage.AlreadyInRole});

            await UserManager.AddToRoleAsync(vm.UserId, roleName);

            return RedirectToAction("UserEdit", new {id = vm.UserId, ViewMessage = ViewMessage.RoleAdded });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveRole(AddRemoveRoleViewmodel vm)
        {
            string roleId = Request.Form["RolesToRemove"];
            string roleName = await db.Roles.Where(r => r.Id == roleId).Select(rn => rn.Name).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(vm.UserId) || string.IsNullOrEmpty(roleId))
                return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.RoleNotRemoved });

            if (!UserManager.IsInRole(vm.UserId, roleName))
                return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.NotInRole });

            await UserManager.RemoveFromRoleAsync(vm.UserId, roleName);

            return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.RoleRemoved });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}
