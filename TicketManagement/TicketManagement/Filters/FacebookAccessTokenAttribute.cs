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
using TicketManagement.Models.Entities;

namespace TicketManagement.Filters
{

    public class FacebookAccessTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ApplicationUserManager userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (userManager != null)
            {
                Task<IList<Claim>> claimsForUser = userManager.GetClaimsAsync(filterContext.HttpContext.User.Identity.GetUserId());

                Claim claim = claimsForUser.Result.FirstOrDefault(c => c.Type == "FacebookAccessToken");
                if (claim != null)
                {
                    string accessToken = claim.Value;

                    if (filterContext.HttpContext.Items.Contains("access_token"))
                        filterContext.HttpContext.Items["access_token"] = accessToken;
                    else
                        filterContext.HttpContext.Items.Add("access_token", accessToken);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
