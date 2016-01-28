using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Management;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [Authorize(Roles = MyRoles.Social)]
    [FacebookAccessToken]
    public class FacebookErrorHandlerController : Controller
    {
        const string RetryCount = "AccessTokenRetryCount";
        const string ProcesssingPermissionRequest = "ProcessingPermissionRequest";

        #region ActionResults

        public ActionResult FacebookError(string errorMessage)
        {
            ViewBag.Type = "Facebook";
            ViewBag.ErrorMessage = errorMessage;
            return PartialView("_Partial_SocialMediaNotLoggedIn");
        }

        public ActionResult RequestNewAccessToken(string facebookUrl)
        {
            ViewBag.FacebookUrl = facebookUrl;
            return PartialView("_Partial_FacebookNewAccessToken");
        }

        [Authorize]
        public async Task<ActionResult> ExternalCallBack(string code)
        {
            FacebookClient fb = new FacebookClient();

            dynamic newTokenResult = await fb.GetTaskAsync($"oauth/access_token?client_id={ConfigurationHelper.GetFacebookAppId()}&client_secret={ConfigurationHelper.GetFacebookAppSecret()}&redirect_uri={Url.Encode(RedirectUri.AbsoluteUri)}&code={code}");

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (userManager != null)
            {
                var userId = HttpContext.User.Identity.GetUserId();

                IList<Claim> currentClaims = await userManager.GetClaimsAsync(userId);

                Claim oldFacebookAccessTokenClaim = currentClaims.FirstOrDefault(c => c.Type == "FacebookAccessToken");

                Claim newFacebookAccessTokenClaim = new Claim("FacebookAccessToken", newTokenResult.access_token);

                if (oldFacebookAccessTokenClaim != null)
                    await userManager.RemoveClaimAsync(userId, oldFacebookAccessTokenClaim);

                await userManager.AddClaimAsync(userId, newFacebookAccessTokenClaim);

                Session.Add(RetryCount, "0");
            }

            return RedirectToAction("Index", "Facebook");
        }

        #endregion

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is FacebookApiLimitException)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Index", "Tickets", new { FacebookError = ErrorType.Exception, Message = "Facebook Graph API limit was reached, please try again later" });
            }
            else if (filterContext.Exception is FacebookOAuthException)
            {
                if (HandleAsExpiredToken((FacebookOAuthException)filterContext.Exception))
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = RedirectToAction("RequestNewAccessToken", new { facebookUrl = GetFacebookLoginUrlString() });
                    //filterContext.Result = GetFacebookLoginUrl();
                }
                else
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = RedirectToAction("Index", "Tickets",
                        new
                        {
                            FacebookError = ErrorType.Exception,
                            Message = $"{filterContext.Exception.Source} controller: {filterContext.Exception.Message}"
                        });
                }
            }
            else if (filterContext.Exception is FacebookApiException)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Index", "Tickets",
                    new
                    {
                        FacebookError = ErrorType.Exception,
                        Message = $"{filterContext.Exception.Source} controller: {filterContext.Exception.Message}"
                    });
            }
            else
                base.OnException(filterContext);
        }

        private bool HandleAsExpiredToken(FacebookOAuthException oAuthException)
        {
            bool handleAsExpiredToken = false;

            if (oAuthException.ErrorCode == 190) // OAuth Exception
            {
                switch (oAuthException.ErrorSubcode)
                {
                    case 458: // App Not Installed
                    case 459: // User Checkpointed
                    case 460: // Password Changed
                    case 463: // Expired
                    case 464: // Unconfirmed User
                    case 467: // Invalid Access Token
                        handleAsExpiredToken = true;
                        break;
                    default:
                        handleAsExpiredToken = false;
                        break;
                }
            }
            else if (oAuthException.ErrorCode == 102) // API Session (Login or access token has expired, been revoked or otherwise invalid).
                handleAsExpiredToken = true;
            else if (oAuthException.ErrorCode == 10) // API Permission Denied (Permission is either not granted or has been removed).
                handleAsExpiredToken = false;
            else if (oAuthException.ErrorCode >= 200 && oAuthException.ErrorCode <= 299)
                handleAsExpiredToken = false;

            return handleAsExpiredToken;
        }

        private string GetFacebookLoginUrlString()
        {
            FacebookClient fb = new FacebookClient();
            fb.AppId = ConfigurationHelper.GetFacebookAppId();

            return fb.GetLoginUrl(new
            {
                scope = ConfigurationHelper.GetFacebookPermissionScope(),
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
            }).ToString();

        }

        private RedirectResult GetFacebookLoginUrl()
        {
            if (Session[RetryCount] == null || (Session[RetryCount] != null && Session[RetryCount].ToString() == "0"))
            {
                Session.Add(RetryCount, "1");

                FacebookClient fb = new FacebookClient();
                fb.AppId = ConfigurationHelper.GetFacebookAppId();

                return Redirect(fb.GetLoginUrl(new
                {
                    scope = ConfigurationHelper.GetFacebookPermissionScope(),
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code",
                }).ToString());
            }
            else
            {
                return Redirect(Url.Action("Index", "TicketCategories")); // TODO: Change this

                //return Redirect(Url.Action("Error", new ErrorViewModel
                //{
                //    Type = ErrorType.Error,
                //    Message = "Unable to obtain a valid Facebook Access Token after multiple attempts, please try again later."
                //}));
            }
        }

        protected Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("ExternalCallBack", "FacebookErrorHandler");
                return uriBuilder.Uri;
            }
        }

        protected string GetAccessToken()
        {
            if (!HttpContext.Items.Contains("access_token"))
            {
                FacebookError("Cannot find your access token, please try re-associating you account with Facebook");
                return null;
            }

            string accessToken = HttpContext.Items["access_token"].ToString();

            if (string.IsNullOrEmpty(accessToken))
            {
                FacebookError("Cannot find your access token, please try re-associating you account with Facebook");
                return null;
            }

            return accessToken;
        }

        protected bool CheckPermission()
        {
            bool checkPermission = true;

            if (TempData[ProcesssingPermissionRequest] != null)
                checkPermission = !((bool)TempData[ProcesssingPermissionRequest]);

            return checkPermission;
        }
    }
}