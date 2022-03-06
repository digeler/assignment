using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    public static class Extensions
    {
        public static bool TryGetClientParam(this NameValueCollection queryString, out string value)
        {
            value = queryString.Get("clientId");

            return !string.IsNullOrEmpty(value);
        }
    }

    public class WebServer
    {
        private volatile bool stop = true;
        UserDb db = new UserDb();

        public async Task StartAsync()
        {

            var perfix = "http://localhost:8080/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(perfix);
            try
            {
                listener.Start();
                Console.WriteLine("listening");

                stop = false;
            }
            catch (HttpListenerException hlex)
            {
                Console.WriteLine((hlex.ErrorCode));
                return;
               
            }
            while (listener.IsListening)
            {
                var context = await listener.GetContextAsync();
                try
                {
                     ProcessRequestAsync(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("# EXCEPTION #   " + ex.StackTrace);
                }
                if (stop) listener.Stop();
            }
            listener.Close();
        }

        public void Stop()
        {
            stop = true;
        }

        private void ProcessRequestAsync(HttpListenerContext context)
        {
            if (context.Request.HttpMethod == "GET" &&
                context.Request.QueryString.TryGetClientParam(out var clientIdValue))
            {
               
                if (db.IsAllowedRequest(clientIdValue, 5))
                {
                    context.Response.StatusCode = 200;
                    Console.WriteLine("success");
                }
                else
                {
                    context.Response.StatusCode = 503;
                    Console.WriteLine("deniel");
                }
            }
            else
            {
                context.Response.StatusCode = 404; //bad request 
                Console.WriteLine("bad requests");
            }

            context.Response.Close();
        }
    }
}