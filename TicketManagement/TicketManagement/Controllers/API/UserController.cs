using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers.API
{
    [AllowAnonymous]
    public class UserController : BaseApiController
    {
        private ApplicationContext db = new ApplicationContext();

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<JsonResult> GetNewUserToken(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            
            User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null) return null;

            if (user.MobileApplicationConfirmed) return null;  // We don't want to give out confirmed User Tokens, so we assume that this is erroneous or malicious.

            user.UserToken = Guid.NewGuid().ToString();
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (string.IsNullOrEmpty(user.UserToken))
                return null;

            return new JsonResult()
            {
                ContentType = "UserTokenAndIsInternal",
                Data = new ApiUserTokenViewModel
                {
                    UserToken = user.UserToken,
                    IsInternal = await IsUserInternal(user.Id)
                }
            };
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<bool> CheckUserToken(string username, string usertoken)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(usertoken)) return false;

            User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null) return false;

            return user.MobileApplicationConfirmed && string.Equals(user.UserToken, usertoken, StringComparison.CurrentCultureIgnoreCase);
        }

        //[System.Web.Http.AcceptVerbs("GET")]
        //public async Task<bool> UserInternal(string username, string usertoken)
        //{
        //    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(usertoken)) return false;

        //    User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);
        //    if (user == null) return false;

        //    return await IsUserInternal(user.Id);
        //}

        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<bool> DeactivateUserToken(string username, string usertoken)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(usertoken)) return false;

            User user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username && u.UserToken == usertoken);
    
            if (user == null) return false;

            user.UserToken = null;
            user.MobileApplicationConfirmed = false;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return true;
        }
    }
}
