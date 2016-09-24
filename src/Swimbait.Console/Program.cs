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
            var config = SwimbaitConfig.Get();
            var environmentService = new EnvironmentService(config);

            //var swimbaitConfig = config.GetSection("Swimbait");

            //var s = swimbaitConfig["RelayHost"];


            var activity = File.ReadAllLines(environmentService.ActivityLogFilename);
            int counter = 1;
            foreach(var line in activity)
            {
                var log = RequestLog.FromCsv(line);

                if (log.Method == "GET")
                {
                    var swimbaitResponse = UriService.GetResponse(environmentService.IpAddress, log.ActualPort, log.PathAndQuery);
                    var yamahaResponse = UriService.GetResponse(IPAddress.Parse("192.168.1.213"), log.YamahaPort, log.PathAndQuery);
                    
                    var logService = new LogService(environmentService);
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
