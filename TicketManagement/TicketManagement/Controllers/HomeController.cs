using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TicketManagement.Helpers;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;
using TicketManagement.Properties;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        #region Properties

        private ApplicationContext db = new ApplicationContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public HomeController()
        {
        }
        public HomeController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
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

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Index(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl)) // There is a return address (problem) and the user is logged in, usually means lack of permissions.
                {
                    ViewBag.ErrorMessage = "It apears that you don't have permission to view that page.";
                    return View("Error");
                }

                return RedirectToAction("Index", "Tickets");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        return RedirectToAction("CheckLogIn", "Home", new { returnUrl = returnUrl });
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", Resources.HomeController_Login_InvalidLoginAttempt);
                    return View(model);
            }
        }

        public ActionResult CheckLogIn(string returnUrl)
        {
            if (!User.IsInRole(MyRoles.Approved))
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.PendingApproval });
            }

            return !string.IsNullOrEmpty(returnUrl) ? RedirectToLocal(returnUrl) : RedirectToAction("Index", "Tickets");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home", new { ViewMessage = ManageMessageId.LoggedOff });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User(model.Email, model.FirstName, model.LastName, model.UserName, await PhoneNumberHelper.FormatPhoneNumberForClockworkAsync(model.PhoneNumber), model.IsArchived);

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("CheckRegister", "Home", new { isInternal = model.IsInternal });
                }

                AddErrors(result);
            }

            return RedirectToAction("Index", "Home", new { ViewMessage = ManageMessageId.Error });
        }

        public async Task<ActionResult> CheckRegister(bool isInternal)
        {
            string userId = User.Identity.GetUserId();
            var user = await db.Users.Where(u => u.Id == userId).Select(u => u).FirstOrDefaultAsync();

            if (isInternal)
            {
                NotificationHelper.AddRoleNotificationToDb(db,
                    new RoleNotification
                    {
                        Role = db.Roles.Where(r => r.Name == MyRoles.Administrator).Select(r => r).FirstOrDefault(),
                        NotificationAboutId = userId,
                        NotificationAbout = user,
                        Type = RoleNotificationType.PendingInternalApproval,
                        Message = NotificationMessages.GetMessageOrNull(RoleNotificationType.PendingInternalApproval)
                    });
            }

            NotificationHelper.AddRoleNotificationToDb(db,
                    new RoleNotification
                    {
                        Role = db.Roles.Where(r => r.Name == MyRoles.Administrator).Select(r => r).FirstOrDefault(),
                        NotificationAboutId = userId,
                        NotificationAbout = user,
                        Type = RoleNotificationType.PendingApproval,
                        Message = NotificationMessages.GetMessageOrNull(RoleNotificationType.PendingApproval)
                    });

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", new { ViewMessage = ViewMessage.PendingApproval });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
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

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}