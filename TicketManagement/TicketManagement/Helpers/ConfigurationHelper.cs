using System;
using System.Configuration;
using System.Threading.Tasks;

namespace TicketManagement.Helpers
{

    public static class ConfigurationHelper
    {
        const int CLOCKWORK_FROM_MAX_LENGTH = 11;

        public static Task<TicketConfiguration> GetTicketConfigurationAsync() { return Task.Factory.StartNew(() => GetTicketConfiguration()); }
        public static TicketConfiguration GetTicketConfiguration()
        {
            TicketConfiguration tconf = new TicketConfiguration();
            return tconf.Populate() ? tconf : null;
        }

        public static Task<string> GetClockworkApiKeyAsync() { return Task.Factory.StartNew(() => GetClockworkApiKey()); }
        public static string GetClockworkApiKey()
        {
            string api = ConfigurationManager.AppSettings["ClockworkAPIKey"];
            return !string.IsNullOrEmpty(api) ? api : null;
        }

        public static Task<string> GetTextMessageFromCodeAsync() { return Task.Factory.StartNew(() => GetTextMessageFromCode()); }
        public static string GetTextMessageFromCode()
        {
            string from = ConfigurationManager.AppSettings["TextMessageFrom"];
            return !string.IsNullOrEmpty(from) ? from : null;
        }

        public static Task<int?> GetTextMessageMaxLengthAsync() { return Task.Factory.StartNew(() => GetTextMessageMaxLength()); }
        public static int? GetTextMessageMaxLength()
        {
            int length;

            if (int.TryParse(ConfigurationManager.AppSettings["TextMessageMaxLength"], out length) && length > 0)
                return length;

            return null;
        }
    }

    public class TicketConfiguration
    {
        public TimeSpan TimeSpanGreen { get; private set; }
        public TimeSpan TimeSpanAmber { get; private set; }
        public TimeSpan TimeSpanRed { get; private set; }

        public Task<bool> PopulateAsync() { return Task.Factory.StartNew(() => Populate()); }
        public bool Populate()
        {
            try
            {
                TimeSpanGreen = TimeSpan.FromHours(int.Parse(ConfigurationManager.AppSettings["TicketTimeSpanGreen"]));
                TimeSpanAmber = TimeSpan.FromHours(int.Parse(ConfigurationManager.AppSettings["TicketTimeSpanAmber"]));
                TimeSpanRed = TimeSpan.FromHours(int.Parse(ConfigurationManager.AppSettings["TicketTimeSpanRed"]));
            }
            catch
            { return false; }

            return true;
        }
    }
}
