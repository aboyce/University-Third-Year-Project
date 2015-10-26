using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingToTFS
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = (args.Length < 1) ? new Uri("http://192.168.1.102:8080/tfs/DefaultCollection/") : new Uri(args[0]);

            ClientObjectModel com = new ClientObjectModel();

            com.LatestCommit(uri);








            Console.ReadLine();
        }
    }
}
