using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Clockwork;
using TicketManagement.Models.Entities;

namespace TicketManagement.Helpers
{
    public class TextMessageHelper
    {
        private string _apiKey = string.Empty;

        private bool LoadInConfiguration()
        {
            if (_apiKey == string.Empty)
                _apiKey = ConfigurationHelper.GetClockworkApiKey();

            return !string.IsNullOrEmpty(_apiKey);
        }


        public Task<string> CheckBalanceAsync() { return Task.Factory.StartNew(CheckBalance); }

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

        public Task<string> SendTextMessageAsync(SentTextMessage txt) { return Task.Factory.StartNew(() => SendTextMessage(txt)); }

        private string SendTextMessage(SentTextMessage txt)
        {
            if (!LoadInConfiguration())
                return "Error: Cannot load details from the web.config";

            try
            {
                API api = new API(_apiKey);
                SMSResult result = api.Send(new SMS
                {
                    To = txt.To,
                    Message = txt.Content,
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

        public ReceivedTextMessage ReceiveTextMessage(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString)) return null;

            XElement xml = XElement.Parse(xmlString);

            string id = (string)xml.Element("Id");
            string to = (string)xml.Element("To");
            string from = (string)xml.Element("From");
            string networkCode = (string)xml.Element("Network");
            string keyword = (string)xml.Element("Keyword");
            string content = (string)xml.Element("Content");

            return new ReceivedTextMessage(to, from, content, id, networkCode, keyword);
        }
    }
}
