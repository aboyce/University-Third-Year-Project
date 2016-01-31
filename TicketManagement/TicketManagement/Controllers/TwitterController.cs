using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Helpers;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace TicketManagement.Controllers
{
    public class TwitterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Authentication()
        {
            ConsumerCredentials credentials = new ConsumerCredentials(await ConfigurationHelper.GetTwitterConsumerKeyAsync(), await ConfigurationHelper.GetTwitterConsumerSecretAsync());

            string url = CredentialsCreator.GetAuthorizationURL(credentials, $"https://{Request.Url.Authority}/Twitter/ValidateAuthentication");

            return new RedirectResult(url);
        }

        public ActionResult ValidateAuthentication()
        {
            string verifierCode = Request.Params.Get("oauth_verifier");
            string authorisationId = Request.Params.Get("authorization_id");

            var credentials = CredentialsCreator.GetCredentialsFromVerifierCode(verifierCode, authorisationId);

            ViewBag.User = Tweetinvi.User.GetLoggedUser(credentials);
            return RedirectToAction("Index");
        }
    }
}