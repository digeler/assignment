using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace server
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            WebServer ws = new WebServer();
            await ws.StartAsync();
        }
    }
}