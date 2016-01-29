using System;
using System.Configuration;
using System.Threading.Tasks;

namespace TicketManagement.Helpers
{

    public static class ConfigurationHelper
    {
        const int CLOCKWORK_FROM_MAX_LENGTH = 11;

        public static Task<TicketConfiguration> GetTicketConfigurationAsync() { return Task.Factory.StartNew(GetTicketConfiguration); }
        public static TicketConfiguration GetTicketConfiguration()
        {
            TicketConfiguration tconf = new TicketConfiguration();
            return tconf.Populate() ? tconf : null;
        }

        public static Task<string> GetFacebookGraphApiVersionAsync() { return Task.Factory.StartNew(GetFacebookGraphApiVersion); }
        public static string GetFacebookGraphApiVersion()
        {
            string value = ConfigurationManager.AppSettings["Facebook_GraphAPIVersion"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetFacebookAppIdAsync() { return Task.Factory.StartNew(GetFacebookAppId); }
        public static string GetFacebookAppId()
        {
            string value = ConfigurationManager.AppSettings["Facebook_AppId"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetFacebookAppSecretAsync() { return Task.Factory.StartNew(GetFacebookAppSecret); }
        public static string GetFacebookAppSecret()
        {
            string value = ConfigurationManager.AppSettings["Facebook_AppSecret"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetFacebookPermissionScopeAsync() { return Task.Factory.StartNew(GetFacebookPermissionScope); }
        public static string GetFacebookPermissionScope()
        {
            string value = ConfigurationManager.AppSettings["Facebook_Permission_Scope"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetFacebookPageIdAsync() { return Task.Factory.StartNew(GetFacebookPageId); }
        public static string GetFacebookPageId()
        {
            string value = ConfigurationManager.AppSettings["Facebook_Admin_Page_Id"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetClockworkApiKeyAsync() { return Task.Factory.StartNew(GetClockworkApiKey); }
        public static string GetClockworkApiKey()
        {
            string value = ConfigurationManager.AppSettings["Clockwork_APIKey"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetTextMessageFromCodeAsync() { return Task.Factory.StartNew(GetTextMessageFromCode); }
        public static string GetTextMessageFromCode()
        {
            string value = ConfigurationManager.AppSettings["TextMessageFrom"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetTextMessageYourNameAsync() { return Task.Factory.StartNew(GetTextMessageYourName); }
        public static string GetTextMessageYourName()
        {
            string value = ConfigurationManager.AppSettings["TextMessage_YourName"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetTextMessageReceiveNumberAsync() { return Task.Factory.StartNew(GetTextMessageReceiveNumber); }
        public static string GetTextMessageReceiveNumber()
        {
            string value = ConfigurationManager.AppSettings["Clockwork_Receive_Number"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<string> GetTextMessageReceiveKeywordAsync() { return Task.Factory.StartNew(GetTextMessageReceiveKeyword); }
        public static string GetTextMessageReceiveKeyword()
        {
            string value = ConfigurationManager.AppSettings["Clockwork_Receive_Keyword"];
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        public static Task<int> GetTextMessageReceiveKeywordLengthAsync() { return Task.Factory.StartNew(GetTextMessageReceiveKeywordLength); }
        public static int GetTextMessageReceiveKeywordLength()
        {
            string value = ConfigurationManager.AppSettings["Clockwork_Receive_Keyword"];
            return string.IsNullOrEmpty(value) ? 0 : value.Length;
        }

        public static Task<int?> GetTextMessageMaxLengthAsync() { return Task.Factory.StartNew(GetTextMessageMaxLength); }
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

        public Task<bool> PopulateAsync() { return Task.Factory.StartNew(Populate); }
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
