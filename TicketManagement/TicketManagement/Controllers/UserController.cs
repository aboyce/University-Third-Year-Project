using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
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
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using User = TicketManagement.Models.Entities.User;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Approved)]
    public class UserController : Controller
    {
        #region Properties

        private ApplicationContext db = new ApplicationContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public UserController()
        {
        }
        public UserController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
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

        #region GET/POST

        public async Task<ActionResult> Index()
        {
            User user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
                return View("Error", new ErrorViewModel { Type = ErrorType.Error, Message = "Could not find user, please try (re)logging in and try again." });

            ViewBag.Teams = new SelectList(db.Teams, "Id", "Name", user.TeamId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "Id,MobileApplicationConfirmed,UserToken,FirstName,LastName,UserName,IsArchived,TeamId,IsTeamLeader,Created,LastUpdated,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (ModelState.IsValid)
            {
                int teamId;
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

                return RedirectToAction("Index", "Tickets", new { ViewMessage = ViewMessage.ProfileUpdated });
            }

            ViewBag.Teams = new SelectList(db.Teams, "Id", "Name", user.TeamId);

            return View(user);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                if (user != null)
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                return RedirectToAction("Index", new { ViewMessage = ManageMessageId.ChangePasswordSuccess });
            }

            AddErrors(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GenerateNewUserToken(string userId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenGenerationFailed });

            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            user.UserToken = Guid.NewGuid().ToString();
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenGenerated });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmUserToken(string userId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenConfirmationFailed });

            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            user.MobileApplicationConfirmed = true;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenConfirmed });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateUserToken(string userId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenDeactivationFailed });

            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            user.MobileApplicationConfirmed = false;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenDeactivated });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TextUserToken(string userId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenSentViaTextFailed });

            User user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            TextMessageHelper textMessageHelper = new TextMessageHelper();
            SentTextMessage textMessage = new SentTextMessage(user.Id, user, user.PhoneNumber, $"Your User Token for {await ConfigurationHelper.GetTextMessageYourNameAsync()} is '{user.UserToken}'");
            string textMessageResult = await textMessageHelper.SendTextMessageAsync(textMessage);

            if (textMessageResult == null)
                RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenSentViaText });

            ViewBag.ErrorMessage = textMessageResult;
            return RedirectToAction("Index", new { ViewMessage = ViewMessage.UserTokenSentViaTextFailed });
        }



        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            IList<UserLoginInfo> userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());

            var facebookConfigured = userLogins.FirstOrDefault(ul => ul.LoginProvider == "Facebook");
            var facebookToConfigure = HttpContext.GetOwinContext().Authentication.GetExternalAuthenticationTypes()
                .Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList().FirstOrDefault(login => login.AuthenticationType == "Facebook");

            IList<Claim> claimsForUser = await UserManager.GetClaimsAsync(User.Identity.GetUserId());
            Claim twitterAccessTokenClaim = claimsForUser.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterAccessToken);

            return View(new ManageLoginsViewModel
            {
                TwitterEnabled = await ConfigurationHelper.IsTwitterConfiguredAsync(),
                TwitterConfigured = twitterAccessTokenClaim != null,
                FacebookToConfigure = facebookToConfigure,
                FacebookConfigured = facebookConfigured
            });
        }

        public async Task<ActionResult> RemoveExternalLoginInformation()
        {
            string userId = User.Identity.GetUserId();

            IList<Claim> currentClaims = await UserManager.GetClaimsAsync(userId);

            foreach (var claim in currentClaims)
                await UserManager.RemoveClaimAsync(userId, claim);

            return RedirectToAction("ManageLogins", new { ViewMessage = ManageMessageId.ClearedExternalLoginInformation });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "User"), User.Identity.GetUserId());
        }

        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);

            if (result.Succeeded)
            {
                User user = await UserManager.FindAsync(loginInfo.Login);

                if (user != null)
                    await StoreFacebookCredentials(user);

                return RedirectToAction("ManageLogins");
            }

            return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                User user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await RemoveFacebookCredentials(user);
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

        public async Task<ActionResult> AuthenticateTwitter()
        {
            ConsumerCredentials credentials = new ConsumerCredentials(await ConfigurationHelper.GetTwitterConsumerKeyAsync(), await ConfigurationHelper.GetTwitterConsumerSecretAsync());

            if (Request.Url == null)
                return RedirectToAction("ManageLogins", new { ViewMessage = ManageMessageId.ErrorWithTwitterAuthentication });

            string url = CredentialsCreator.GetAuthorizationURL(credentials, $"https://{Request.Url.Authority}/User/ValidateTwitterAuthentication"); // Check that Url matched the method below.

            return new RedirectResult(url);
        }

        public async Task<ActionResult> ValidateTwitterAuthentication()
        {
            string verifierCode = Request.Params.Get("oauth_verifier");
            string authorisationId = Request.Params.Get("authorization_id");

            ITwitterCredentials credentials = CredentialsCreator.GetCredentialsFromVerifierCode(verifierCode, authorisationId);

            await StoreTwitterCredentials(verifierCode, authorisationId, credentials.AccessToken, credentials.AccessTokenSecret);

            return RedirectToAction("ManageLogins");
        }

        private async Task StoreTwitterCredentials(string verifierCode, string authorisationId, string accessToken, string accessTokenSecret)
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (userManager != null)
            {
                string userId = User.Identity.GetUserId();

                IList<Claim> currentClaims = await userManager.GetClaimsAsync(userId);

                Claim oldTwitterVerifierCodeClaim = currentClaims.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterVerifierCode);
                Claim oldTwitterAuthorisationIdClaim = currentClaims.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterAuthorisationId);
                Claim oldTwitterAccessTokenClaim = currentClaims.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterAccessToken);
                Claim oldTwitterAccessTokenSecretClaim = currentClaims.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterAccessTokenSecret);

                Claim newTwitterVerifierCodeClaim = new Claim(SocialMediaItem.TwitterVerifierCode, verifierCode);
                Claim newTwitterAuthorisationIdClaim = new Claim(SocialMediaItem.TwitterAuthorisationId, authorisationId);
                Claim newTwitterAccessTokenClaim = new Claim(SocialMediaItem.TwitterAccessToken, accessToken);
                Claim newTwitterAccessTokenSecretClaim = new Claim(SocialMediaItem.TwitterAccessTokenSecret, accessTokenSecret);

                if (oldTwitterVerifierCodeClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldTwitterVerifierCodeClaim);
                if (oldTwitterAuthorisationIdClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldTwitterAuthorisationIdClaim);
                if (oldTwitterAccessTokenClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldTwitterAccessTokenClaim);
                if (oldTwitterAccessTokenSecretClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldTwitterAccessTokenSecretClaim);

                await userManager.AddClaimAsync(userId, newTwitterVerifierCodeClaim);
                await userManager.AddClaimAsync(userId, newTwitterAuthorisationIdClaim);
                await userManager.AddClaimAsync(userId, newTwitterAccessTokenClaim);
                await userManager.AddClaimAsync(userId, newTwitterAccessTokenSecretClaim);
            }
        }

        public async Task<ActionResult> RemoveTwitterAuthentication()
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (userManager != null)
            {
                string userId = User.Identity.GetUserId();

                IList<Claim> currentClaims = await userManager.GetClaimsAsync(userId);

                Claim oldTwitterVerifierCodeClaim = currentClaims.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterVerifierCode);
                Claim oldTwitterAuthorisationIdClaim = currentClaims.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterAuthorisationId);
                Claim oldTwitterAccessTokenClaim = currentClaims.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterAccessToken);
                Claim oldTwitterAccessTokenSecretClaim = currentClaims.FirstOrDefault(c => c.Type == SocialMediaItem.TwitterAccessTokenSecret);

                if (oldTwitterVerifierCodeClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldTwitterVerifierCodeClaim);
                if (oldTwitterAuthorisationIdClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldTwitterAuthorisationIdClaim);
                if (oldTwitterAccessTokenClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldTwitterAccessTokenClaim);
                if (oldTwitterAccessTokenSecretClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldTwitterAccessTokenSecretClaim);
            }

            return RedirectToAction("ManageLogins");
        }

        private async Task StoreFacebookCredentials(User user)
        {
            ClaimsIdentity claimsIdentity = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            if (claimsIdentity != null)
            {
                IList<Claim> currentClaims = await UserManager.GetClaimsAsync(user.Id);

                if (currentClaims.Any())
                    await RemoveFacebookCredentials(user);

                Claim facebookAccessToken = claimsIdentity.FindAll(SocialMediaItem.FacebookAccessToken).FirstOrDefault();
                Claim facebookPageAccessToken = claimsIdentity.FindAll(SocialMediaItem.FacebookPageAccessToken).FirstOrDefault();

                await UserManager.AddClaimAsync(user.Id, facebookAccessToken);
                await UserManager.AddClaimAsync(user.Id, facebookPageAccessToken);
            }
        }

        private async Task RemoveFacebookCredentials(User user)
        {
            ClaimsIdentity claimsIdentity = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            if (claimsIdentity != null)
            {
                IList<Claim> currentClaims = await UserManager.GetClaimsAsync(user.Id);

                foreach (var claim in currentClaims)
                {
                    if (claim.Type == SocialMediaItem.FacebookAccessToken || claim.Type == SocialMediaItem.FacebookPageAccessToken)
                        await UserManager.RemoveClaimAsync(user.Id, claim);
                }
            }
        }

        #endregion

        #region ExternalLogin

        //public async Task<ActionResult> ExternalLogins(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : "";

        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

        //    if (user == null)
        //        return View("Error", new ErrorViewModel { Type = ErrorType.Error, Message = "Problem with linking the external log in, please try again." });

        //    var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
        //    var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
        //    ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;

        //    return View(new ExternalLoginsViewModel() // TODO: Investigate if this is a problem.
        //    {
        //        CurrentLogins = userLogins,
        //        OtherLogins = otherLogins
        //    });
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "User", new { ReturnUrl = returnUrl }));
        //}

        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

        //    if (loginInfo == null)
        //        return View("Error", new ErrorViewModel { Type = ErrorType.Error, Message = "Struggling to get anything back from external login, please try again." });

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //        //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            //return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //            return View("ExternalLoginConfirmation", new RegisterViewModel { Email = loginInfo.Email, UserName = loginInfo.DefaultUserName });
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(RegisterViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //        return RedirectToAction("Index", "Tickets");

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();

        //        if (info == null)
        //            return View("Error", new ErrorViewModel { Type = ErrorType.Error, Message = "Unsuccessful login with service, please try again." });

        //        User user = new User(model.Email, model.FirstName, model.LastName, model.UserName, await PhoneNumberHelper.FormatPhoneNumberForClockworkAsync(model.PhoneNumber), model.IsArchived);

        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToAction("CheckRegister", "Home", new { isInternal = model.IsInternal });
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        // ------------------------------------------------------------------------------------------------------------------

        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}
        ////
        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        ////
        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new User { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        #endregion

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
            return RedirectToAction("Index", "User");
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

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

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
