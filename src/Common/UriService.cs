using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Swimbait.Server.Services;

namespace Swimbait.Common
{
    public class UriService
    {
        public static ResponseLog GetResponse(string targetIp, int port, string pathAndQuery)
        {
            var result = new ResponseLog();
            using (var httpClient = new HttpClient())
            {
                var uri = new Uri($"http://{targetIp}:{port}" + pathAndQuery);

                result.RequestUri = uri;

                try
                {
                    result.ResponseBody = httpClient.GetStringAsync(uri).Result;
                }
                catch (Exception e)
                {
                    result.ResponseBody = e.Message;
                }
            }
            return result;
        }

        public static ResponseLog GetManInTheMiddleResult(string targetIp, Uri thisRequest, Func<Uri,int> portRemap)
        {
            var relayPort = portRemap(thisRequest);
            var result = GetResponse(targetIp, relayPort, thisRequest.PathAndQuery);
            return result;
        }

    }
}
