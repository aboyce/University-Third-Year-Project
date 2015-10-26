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

            Console.WriteLine("*********************************************************");
            Console.WriteLine("CLIENT OBJECT MODEL");
            ClientObjectModel com = new ClientObjectModel();

            Console.WriteLine("Latest Commit");
            Console.WriteLine("---------------------------------------------------------");
            Uri comUri = (args.Length < 1) ? new Uri("http://192.168.1.102:8080/tfs/DefaultCollection/") : new Uri(args[0]);
            com.LatestCommit(comUri);
            Console.WriteLine("---------------------------------------------------------");

            
            Console.WriteLine("*********************************************************");
            Console.WriteLine("REST API");
            RestApi restApi = new RestApi();

            Console.WriteLine("Changesets From the Last Week");
            Console.WriteLine("---------------------------------------------------------");
            string restUri = "http://192.168.1.102:8080/tfs/defaultcollection/_apis/tfvc/changesets?fromDate=" +
                          DateTime.Now.AddDays(-7).ToString("yyyy-M-d");
            restApi.LatestCommit(restUri);
            Console.WriteLine("---------------------------------------------------------");

            Console.ReadLine();

        }
    }
}
