using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public ActionResult Authentication()
        {
            ConsumerCredentials credentials = new ConsumerCredentials("l9gnPI0cjCCa1STlJ8ySNP7Sb", "fQhKIM2sVFaS84d0mvAOUuZsIxB4Kg9jXGwiwPPmsuWqeCGIgW");

            string url = CredentialsCreator.GetAuthorizationURL(credentials, $"https://{Request.Url.Authority}/Twitter/ValidateAuthentication");

            return new RedirectResult(url);
        }

        public ActionResult ValidateAuthentication()
        {
            var verifierCode = Request.Params.Get("oauth_verifier");
            var authorisationId = Request.Params.Get("authorization_id");

            var credentials = CredentialsCreator.GetCredentialsFromVerifierCode(verifierCode, authorisationId);

            ViewBag.User = Tweetinvi.User.GetLoggedUser(credentials);
            return View();
        }
    }
}