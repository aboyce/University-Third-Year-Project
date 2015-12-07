using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Clockwork;

namespace TicketManagement.Helpers
{
    class TextMessageManager
    {
        public string SendTextMessage(string number, string message)
        {
            try
            {
                string apiKey = Helpers.Configuration.GetClockworkApiKey();
                string from = Helpers.Configuration.GetTextMessageFromCode();

                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(from))
                    return "Cannot load the API key or From value from the web.config";

                API api = new API(apiKey);
                SMSResult result = api.Send(new SMS
                {
                    To = number,
                    Message = message,
                    From = from
                });

                return result.Success ? null : $"Failed to send message {result.ID} because {result.ErrorMessage} (Code: {result.ErrorCode})";
            }
            catch (APIException ex)
            {
                // You’ll get an API exception for errors
                // such as wrong username or password
                return "API Exception: " + ex.Message;
            }
            catch (WebException ex)
            {
                // Web exceptions mean you couldn’t reach the Clockwork server
                return "Web Exception: " + ex.Message;
            }
            catch (ArgumentException ex)
            {
                // Argument exceptions are thrown for missing parameters,
                // such as forgetting to set the username
                return "Argument Exception: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Something else went wrong, the error message should help
                return "Unknown Exception: " + ex.Message;
            }
        }
    }
}
