using System;
using System.IO;
using System.Net;
using Swimbait.Common;
using Swimbait.Common.Services;

namespace Swimbait.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var environmentService = new EnvironmentService();

            const string activityLog = @"D:\Downloads\swimbait\activity.txt";
            var activity = File.ReadAllLines(activityLog);
            int counter = 1;
            foreach(var line in activity)
            {
                var log = RequestLog.FromCsv(line);

                if (log.Method == "GET")
                {
                    var swimbaitResponse = UriService.GetResponse(environmentService.IpAddress, log.ActualPort, log.PathAndQuery);
                    var yamahaResponse = UriService.GetResponse(IPAddress.Parse("192.168.1.213"), log.YamahaPort, log.PathAndQuery);
                    
                    var logService = new LogService();
                    logService.LogToDisk(counter, swimbaitResponse);
                    logService.LogToDisk(counter, yamahaResponse);
                }

                counter++;
            }
        }

        public static int MapPortToReal(Uri thisRequest)
        {
            // remap the port since windows is using the port Yamaha uses
            var relayPort = thisRequest.Port == 51123 ? EnvironmentService.YamahaDlnaPort : thisRequest.Port;
            return relayPort;
        }
    }
}
