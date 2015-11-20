using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Helpers
{
    public static class Configuration
    {
        public static TicketConfiguration GetTicketConfiguration()
        {
            TicketConfiguration tconf = new TicketConfiguration();
            return tconf.Populate() ? tconf : null;
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
                TimeSpanGreen = TimeSpan.FromMinutes(int.Parse(System.Configuration.ConfigurationManager.AppSettings["TicketTimeSpanGreen"]));
                TimeSpanAmber = TimeSpan.FromMinutes(int.Parse(System.Configuration.ConfigurationManager.AppSettings["TicketTimeSpanAmber"]));
                TimeSpanRed = TimeSpan.FromMinutes(int.Parse(System.Configuration.ConfigurationManager.AppSettings["TicketTimeSpanRed"]));
            }
            catch
            { return false; }

            return true;
        }
    }
}
