using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Clockwork;
using TicketManagement.Models.Entities;

namespace TicketManagement.Helpers
{
    public class TextMessageManager
    {
        private string _apiKey = string.Empty;

        private bool LoadInConfiguration()
        {
            if (_apiKey == string.Empty)
                _apiKey = Helpers.Configuration.GetClockworkApiKey();

            return !string.IsNullOrEmpty(_apiKey);
        }


        public string CheckBalance()
        {
            if (!LoadInConfiguration())
                return "Error: Cannot load details from the web.config";

            try
            {
                API api = new API(_apiKey);
                Balance balance = api.GetBalance();
                return balance.CurrencySymbol + balance.Amount.ToString("#,0.00");
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

        public string SendTextMessage(TextMessage txt)
        {
            if (!LoadInConfiguration())
                return "Error: Cannot load details from the web.config";

            try
            {
                API api = new API(_apiKey);
                SMSResult result = api.Send(new SMS
                {
                    To = txt.Number,
                    Message = txt.Message,
                    From = txt.From
                });

                return result.Success ? null : $"Error: Failed to send message {result.ID} because {result.ErrorMessage} (Code: {result.ErrorCode})";
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
