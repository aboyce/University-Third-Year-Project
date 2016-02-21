using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextMessageProtocol
{
    class Program
    {
        static void Main(string[] args)
        {
            TextMessageProtocolHelper protocol = new TextMessageProtocolHelper();
            protocol.ProcessTextMessageRegardingProtcols(args[0]);

            Console.ReadLine();
        }

        public static class TextMessageProtocol
        {
            // CONFIRM_USER_TOKEN:{user_token}
            public const string ConfirmUserToken = "CONFIRM_USER_TOKEN";
        }

        public class TextMessageProtocolHelper
        {
            // At this stage the Keyword at the start of the Text Message should have been removed.

            public bool ProcessTextMessageRegardingProtcols(string textMessageBody)
            {
                if (textMessageBody.Contains(TextMessageProtocol.ConfirmUserToken))
                    return ProcessConfirmUserToken(textMessageBody);

                return true;
            }

            private bool ProcessConfirmUserToken(string textMessageBody)
            {
                string[] currentSplit = textMessageBody.Split(new[] { TextMessageProtocol.ConfirmUserToken }, StringSplitOptions.None);

                if (currentSplit.Length > 2)
                    return false; // We are not going to handle this if they have put in multiple instances of the keyword, as unsure on which one is intented.

                if (currentSplit.Length == 2) // We found one instance of 'Confirm User Token Keyword'.
                {
                    string userToken = currentSplit[1].Replace(":", ""); // Remove the ':' that is splitting up the keywork and parameter.
                    currentSplit = userToken.Split(new [] { " " }, StringSplitOptions.None); // There still may be more content after the parameter.
                    if (currentSplit.Length >= 0)
                        userToken = currentSplit[0];
                            // We only need the parameter for the keyword, so in this case we can forget anything after the parameter.
                    else
                        return false;



                }


                return true;
            }


        }
    }
}
