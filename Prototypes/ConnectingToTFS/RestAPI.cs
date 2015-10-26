using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingToTFS
{
    class RestApi
    {
        public void LatestCommit(string uri)
        {
            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = client.GetAsync(uri).Result)
                {
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }
        }
    }
}
