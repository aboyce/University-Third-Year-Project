using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListOfValuesFromClass
{
    public static class TextMessageProtocol
    {
        public static readonly string HelpText = "HELP_TEXT";
        public static readonly string ConfirmUserToken = "CONFIRM_USER_TOKEN";
        public static readonly string GetNotifications = "GET_NOTIFICATIONS";
        public static readonly string ExternalTicket = "EXTERNAL_TICKET";
    }

    class Program
    {
        static void Main(string[] args)
        {
            GetTextMessageProtocolTypes();
        }

        private static void GetTextMessageProtocolTypes()
        {
            string protocolTypes = "";
            Type type = typeof(TextMessageProtocol);

            foreach (FieldInfo property in type.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
                protocolTypes += property.GetValue(null).ToString();

            Console.WriteLine(protocolTypes);
        }
    }
}
