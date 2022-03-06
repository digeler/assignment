using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace server
{
    public class UserDb
    {
       
        private ConcurrentDictionary<string, (DateTime TimeOfFirstRequest, int RequestCount)> ClientRequestsData { get; } = 
            new ConcurrentDictionary<string, (DateTime, int)>();
        private TimeSpan TimeFrame { get; } = new TimeSpan(0, 0, 0, 5);

        public bool IsAllowedRequest(string clientid, int threshold)
        {
           if (ClientRequestsData.TryGetValue(clientid,out var clientdata))
            {
                if ((DateTime.Now - clientdata.TimeOfFirstRequest) < TimeFrame)
                {
                    if (clientdata.RequestCount < threshold)

                    {
                        ClientRequestsData.TryUpdate(clientid, (clientdata.TimeOfFirstRequest,clientdata.RequestCount+1), clientdata);

                        return true;
                    }
                }
                else
                {
                    ClientRequestsData.TryUpdate(clientid, (DateTime.Now, 1), clientdata);
                    return true;
                }
            }
           else
           {
               ClientRequestsData.TryAdd(clientid, (DateTime.Now, 1));

               return true;
           }

            return false;
        }
       
    }
}