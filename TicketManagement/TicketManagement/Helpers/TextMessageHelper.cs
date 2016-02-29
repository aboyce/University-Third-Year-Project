using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Clockwork;
using TicketManagement.Models.Context;
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

            try
            {
                XElement xml = XElement.Parse(xmlString);

                string id = (string)xml.Element("Id");
                string to = (string)xml.Element("To");
                string from = (string)xml.Element("From");
                string networkCode = (string)xml.Element("Network");
                string keyword = (string)xml.Element("Keyword");
                string content = (string)xml.Element("Content");

                return new ReceivedTextMessage(to, from, content, id, networkCode, keyword);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ReceivedTextMessage> ProcessTextMessage(ReceivedTextMessage txt, ApplicationContext db)
        {          
            if (txt.To != null && string.Equals(txt.To, await ConfigurationHelper.GetTextMessageReceiveNumberAsync(), StringComparison.CurrentCultureIgnoreCase))
            {
                txt.To = await ConfigurationHelper.GetTextMessageYourNameAsync() ?? txt.To; // Try and get the correct name from the config, if its null (missing) leave it as it came in.

                if (string.IsNullOrEmpty(txt.Content)) // If the txt.To matches the number above, we must have a Keyword added from Clockwork, we want to remove this.
                    txt.Content = txt.Content.Remove(0, await ConfigurationHelper.GetTextMessageReceiveKeywordLengthAsync());
            }

            if (txt.From != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == txt.From);

                if (user != null)
                {
                    txt.UserFrom = user;
                    txt.UserFromId = user.Id;
                }
            }

            if (txt.Content != null)
            {
                // Removes the initial string that is required by Clockwork to direct the message to the system on the shared number.
                txt.Content = txt.Content.Replace($"{await ConfigurationHelper.GetTextMessageReceiveKeywordAsync()} ", "");

                TextMessageProtocolHelper textMessageProtocol = new TextMessageProtocolHelper(db);
                await textMessageProtocol.ProcessTextMessageRegardingProtcols(txt);
            }

            return txt;
        }
    }
}
