using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using Newtonsoft.Json;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers.API
{
    [AllowAnonymous]
    public class UserController : BaseApiController
    {
        private ApplicationContext db = new ApplicationContext();

        public async Task<string> GetNewUserToken(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user.MobileApplicationConfirmed)
                return null;  // We don't want to give out confirmed User Tokens, so we assume that this is erroneous or malicious.

            user.UserToken = Guid.NewGuid().ToString();
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            return string.IsNullOrEmpty(user.UserToken) ? null : user.UserToken;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<bool> ClearUserToken(string username, string userToken)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userToken))
                return false;

            User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username && u.UserToken == userToken);

            if (user == null)
                return false;

            user.UserToken = null;
            user.MobileApplicationConfirmed = false;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return true;
        }
    }
}
