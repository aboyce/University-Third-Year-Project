using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TicketManagement.ViewModels;
using TicketManagement.Models.Context;
using System.Web.Security;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketManagement.Helpers;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;


namespace TicketManagement.Controllers
{
    [Authorize(Roles="Administrator")]
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

        public ActionResult Index()
        {
            Dictionary<char, int> totals = new Dictionary<char, int>
            {
                {'o', db.Organisations.Select(o => o.Id).Count()},
                {'u', db.Users.Select(u => u.Id).Count()},
                {'p', db.Projects.Select(p => p.Id).Count()},
                {'m', db.Teams.Select(m => m.Id).Count()},
                {'t', db.Tickets.Select(t => t.Id).Count()},
                {'g', db.TicketCategories.Select(g => g.Id).Count()},
                {'l', db.TicketLogs.Select(l => l.Id).Count()},
                {'y', db.TicketLogTypes.Select(y => y.Id).Count()},
                {'i', db.TicketPriorities.Select(i => i.Id).Count()},
                {'e', db.TicketStates.Select(e => e.Id).Count()},
            };

            TicketConfiguration conf = ConfigurationHelper.GetTicketConfiguration();

            if (conf == null) return View(totals);

            totals.Add('1', (int)conf.TimeSpanGreen.TotalHours);
            totals.Add('2', (int)conf.TimeSpanAmber.TotalHours);
            totals.Add('3', (int)conf.TimeSpanRed.TotalHours);

            return View(totals);
        }

        //
        // GET: Users
        public ActionResult Users()
        {
            return View(db.Users.Select(u => u).ToList().ToDictionary(user => user, user => UserManager.GetRoles(user.Id)));
        }
        
        //
        // GET: UserEdit
        public ActionResult UserEdit(string id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "'Admin/UserEdit' has been visited from the wrong location, please ensure you are an admin and try again.";
                return View("Error");
            }

            User user = UserManager.FindById(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Could not find user, please try again.";
                return View("Error");
            }

            ViewBag.Teams = new SelectList(db.Teams, "Id", "Name", user.TeamId);

            //var roles = (from role in db.Roles.ToList() where role.Users.Count > 0 from userRole in role.Users where userRole.UserId == user.Id select role.Name);

            ViewBag.RolesToAdd = new SelectList(db.Roles, "Id", "Name", user.Roles);
            ViewBag.RolesToRemove = new SelectList(db.Roles, "Id", "Name", user.Roles);

            return View(user);

        }

        // POST: UserEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEdit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                int teamId = 0;
                Team team = null;

                if (int.TryParse(Request.Form["Teams"], out teamId))
                    team = db.Teams.FirstOrDefault(t => t.Id == teamId);

                if (team != null)
                {
                    user.Team = team;
                    user.TeamId = team.Id;
                }
                
                user.PhoneNumber = PhoneNumberHelper.FormatPhoneNumberForClockwork(user.PhoneNumber);
                user.LastUpdated = DateTime.Now;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Users", "Admin", new { ViewMessage = ViewMessage.ProfileUpdated });
            }

            ViewBag.Teams = new SelectList(db.Teams, "Id", "Name", user.TeamId);
            return View(user);
        }

        // POST: AddRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(AddRemoveRoleViewmodel vm)
        {
            string roleId = Request.Form["RolesToAdd"];
            string roleName = db.Roles.Where(r => r.Id == roleId).Select(rn => rn.Name).FirstOrDefault();

            if (string.IsNullOrEmpty(vm.UserId) || string.IsNullOrEmpty(roleId))
                return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.RoleNotAdded });

            if (UserManager.IsInRole(vm.UserId, roleName))
                return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.AlreadyInRole });

            UserManager.AddToRole(vm.UserId, roleName);

            return RedirectToAction("UserEdit", new {id = vm.UserId, ViewMessage = ViewMessage.RoleAdded });
        }

        // POST: RemoveRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveRole(AddRemoveRoleViewmodel vm)
        {
            string roleId = Request.Form["RolesToRemove"];
            string roleName = db.Roles.Where(r => r.Id == roleId).Select(rn => rn.Name).FirstOrDefault();

            if (string.IsNullOrEmpty(vm.UserId) || string.IsNullOrEmpty(roleId))
                return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.RoleNotRemoved });

            if (!UserManager.IsInRole(vm.UserId, roleName))
                return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.NotInRole });

            UserManager.RemoveFromRole(vm.UserId, roleName);

            return RedirectToAction("UserEdit", new { id = vm.UserId, ViewMessage = ViewMessage.RoleRemoved });
        }


        //
        // POST: /Admin/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { ViewMessage = message });
        }

        //
        // GET: /Admin/ExternalLogins
        public async Task<ActionResult> ExternalLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ExternalLoginsViewModel()
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
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
