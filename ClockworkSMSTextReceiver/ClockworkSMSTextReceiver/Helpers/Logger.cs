using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockworkSMSTextReceiver.Helpers
{
    public enum LogType
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public static class Logger
    {
        public static void Log(LogType type, string message)
        {
            Console.WriteLine($"{type}: {message}");
        }
    }
}
