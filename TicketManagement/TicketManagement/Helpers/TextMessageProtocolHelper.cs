using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Context;

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


        public bool ProcessTextMessageRegardingProtcols(string textMessageBody)
        {
            if (textMessageBody.Contains(TextMessageProtocol.ConfirmUserToken))
                return ProcessConfirmUserToken(textMessageBody);

            return true;
        }

        private bool ProcessConfirmUserToken(string textMessageBody)
        {
            string[] splitTextMessageBody = textMessageBody.Split(new string[] {TextMessageProtocol.ConfirmUserToken}, StringSplitOptions.None); 



            return true;
        }


    }
}
