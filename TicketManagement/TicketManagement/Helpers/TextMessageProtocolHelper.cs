using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;

namespace TicketManagement.Helpers
{
    public static class TextMessageProtocol
    {
        // CONFIRM_USER_TOKEN:{user_token}
        public const string ConfirmUserToken = "CONFIRM_USER_TOKEN";
    }

    public class TextMessageProtocolHelper
    {
        private ApplicationContext db;

        public TextMessageProtocolHelper(ApplicationContext dbContext)
        {
            db = dbContext;
        }


        public bool ProcessTextMessageRegardingProtcols(ReceivedTextMessage txt)
        {
            if (txt == null) return false;

            if (txt.Content.Contains(TextMessageProtocol.ConfirmUserToken))
                return ProcessConfirmUserToken(txt.From, txt.Content);

            return true;
        }

        private bool ProcessConfirmUserToken(string phoneNumber, string textMessageBody)
        {
            string[] currentSplit = textMessageBody.Split(new[] { TextMessageProtocol.ConfirmUserToken }, StringSplitOptions.None);

            if (currentSplit.Length > 2)
                return false; // We are not going to handle this if they have put in multiple instances of the keyword, as unsure on which one is intented.

            if (currentSplit.Length != 2) return true; // If there is more than one instance of the keyword we will not processes it.

            string userToken = currentSplit[1].Replace(":", "");                        // Remove the ':' that is splitting up the keywork and parameter.
            currentSplit = userToken.Split(new[] { " " }, StringSplitOptions.None);     // There still may be more content after the parameter.

            if (currentSplit.Length < 0)
                return false;

            userToken = currentSplit[0]; // We only need the parameter for the keyword, so in this case we can forget anything after the parameter.

            User user = db.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null) return false;
            if (user.UserToken != userToken) return false;

            user.MobileApplicationConfirmed = true;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }


    }
}
