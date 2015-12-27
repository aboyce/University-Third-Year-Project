using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ClockworkSMSTextReceiver.Helpers;

namespace ClockworkSMSTextReceiver
{
    public class Server
    {
        private HttpListener _listener;

        public async void StartServer()
        {
            _listener = new HttpListener();

            try
            {
                _listener.Prefixes.Add("http://127.0.0.1/");

                Logger.Log(LogType.Info, "Prefix added");

                _listener.Start();

                Logger.Log(LogType.Info, "Listener Started");

                while (_listener.IsListening)
                {
                    HttpListenerContext context = await _listener.GetContextAsync();

                    //string rawData = await new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEndAsync();

                    string test = HttpUtility.UrlDecode(new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd());



                }

            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }

        }

        public void StopServer()
        {
            _listener.Stop();
            _listener.Close();
        }


    }
}
