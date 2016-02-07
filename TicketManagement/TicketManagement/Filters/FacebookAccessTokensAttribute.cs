using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TicketManagement.Management;

namespace TicketManagement.Filters
{

    public class FacebookAccessTokensAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ApplicationUserManager userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (userManager != null)
            {
                Task<IList<Claim>> claimsForUser = userManager.GetClaimsAsync(filterContext.HttpContext.User.Identity.GetUserId());

                Claim userClaim = claimsForUser.Result.FirstOrDefault(c => c.Type == SocialMediaItem.FacebookAccessToken);

                Claim pageClaim = claimsForUser.Result.FirstOrDefault(c => c.Type == SocialMediaItem.FacebookPageAccessToken);

                if (userClaim != null)
                {
                    string accessToken = userClaim.Value;

                    if (filterContext.HttpContext.Items.Contains(SocialMediaItem.FacebookAccessToken))
                        filterContext.HttpContext.Items[SocialMediaItem.FacebookAccessToken] = accessToken;
                    else
                        filterContext.HttpContext.Items.Add(SocialMediaItem.FacebookAccessToken, accessToken);
                }

                if (pageClaim != null)
                {
                    string accessToken = pageClaim.Value;

                    if (filterContext.HttpContext.Items.Contains(SocialMediaItem.FacebookPageAccessToken))
                        filterContext.HttpContext.Items[SocialMediaItem.FacebookPageAccessToken] = accessToken;
                    else
                        filterContext.HttpContext.Items.Add(SocialMediaItem.FacebookPageAccessToken, accessToken);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
