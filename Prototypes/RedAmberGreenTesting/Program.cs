using System;
using System.Collections.Generic;

namespace RedAmberGreenTesting
{
    class Program
    {
        private static void Main(string[] args)
        {
            List<DateTime> times = new List<DateTime>();

            DateTime now = DateTime.Now;
            DateTime temp = DateTime.Now;

            times.Add(temp); // null
            temp = DateTime.Now;

            times.Add(temp.AddHours(12)); // null
            temp = DateTime.Now;

            times.Add(temp.AddHours(26)); // green
            temp = DateTime.Now;

            times.Add(temp.AddHours(49)); // amber
            temp = DateTime.Now;

            times.Add(temp.AddHours(105)); // red
            temp = DateTime.Now;

            times.Add(temp.AddHours(90)); // amber
            temp = DateTime.Now;

            times.Add(temp.AddHours(24)); // green
            temp = DateTime.Now;

            times.Add(temp.AddHours(580)); // red
            temp = DateTime.Now;

            TimeSpan green = TimeSpan.FromHours(24);
            TimeSpan amber = TimeSpan.FromHours(48);
            TimeSpan red = TimeSpan.FromHours(100);

            string rowColour = "*\n";

            foreach (DateTime time in times)
            {
                if ((now - time).Duration() >= red)
                {
                    rowColour += "red\n";
                }
                else if ((now - time).Duration() >= amber)
                {
                    rowColour += "amber\n";
                }
                else if ((now - time).Duration() >= green)
                {
                    rowColour += "green\n";
                }
                else
                {
                    rowColour += "null\n";
                }
            }

            Console.WriteLine(rowColour);
            Console.ReadLine();
        }
    }
}
