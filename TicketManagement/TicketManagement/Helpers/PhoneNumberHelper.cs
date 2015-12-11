using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Helpers
{
    public static class PhoneNumberHelper
    {
        /// <summary>
        /// Method to check that a mobile number either starts with '44', if not will replace '07' with '447'.
        /// </summary>
        /// <param name="number">The phone number to be formatted</param>
        /// <returns>Either the parameter if it is ok or incorrect, else the modified number.</returns>
        public static string FormatPhoneNumberForClockwork(string number)
        {
            char [] numberArray = number.ToCharArray();

            if (numberArray.Length < 11)
                return number;

            if (numberArray[0] == '4' && numberArray[1] == '4')
                return number;

            if (numberArray[0] == '0' && numberArray[1] == '7')
                return "44" + number.Substring(1);

            return number; // Null will cause a problem for the database.
        }
    }
}
