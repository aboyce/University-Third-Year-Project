using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;

namespace TicketManagement.Helpers
{

    public static class ConfigurationHelper
    {
        const int CLOCKWORK_FROM_MAX_LENGTH = 11;

        public static TicketConfiguration GetTicketConfiguration()
        {
            TicketConfiguration tconf = new TicketConfiguration();
            return tconf.Populate() ? tconf : null;
        }

        /// <summary>
        /// Will try to get the Clockwork API key from the Web.Config
        /// </summary>
        /// <returns>The key if it is found, null if not.</returns>
        public static string GetClockworkApiKey()
        {
            string api = System.Configuration.ConfigurationManager.AppSettings["ClockworkAPIKey"];
            return !string.IsNullOrEmpty(api) ? api : null;
        }

        /// <summary>
        /// Will try to get the 'from' work from the Web.Config
        /// </summary>
        /// <returns>The value if it is found, null if not.</returns>
        public static string GetTextMessageFromCode()
        {
            string from = System.Configuration.ConfigurationManager.AppSettings["TextMessageFrom"];

            //from = from.Substring(0, CLOCKWORK_FROM_MAX_LENGTH);

            return !string.IsNullOrEmpty(from) ? from : null;
        }

        public static int? GetTextMessageMaxLength()
        {
            int length;

            if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["TextMessageMaxLength"], out length) && length > 0)
                return length;

            return null;
        }

    }

    public class TicketConfiguration
    {
        public TimeSpan TimeSpanGreen { get; private set; }
        public TimeSpan TimeSpanAmber { get; private set; }
        public TimeSpan TimeSpanRed { get; private set; }

        public bool Populate()
        {
            try
            {
                TimeSpanGreen = TimeSpan.FromHours(int.Parse(System.Configuration.ConfigurationManager.AppSettings["TicketTimeSpanGreen"]));
                TimeSpanAmber = TimeSpan.FromHours(int.Parse(System.Configuration.ConfigurationManager.AppSettings["TicketTimeSpanAmber"]));
                TimeSpanRed = TimeSpan.FromHours(int.Parse(System.Configuration.ConfigurationManager.AppSettings["TicketTimeSpanRed"]));
            }
            catch
            { return false; }

            return true;
        }
    }
}
