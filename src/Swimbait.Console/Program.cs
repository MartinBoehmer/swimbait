using System;
using System.IO;
using Swimbait.Common;

namespace Swimbait.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string activityLog = @"D:\Downloads\swimbait\activity.txt";
            var activity = File.ReadAllLines(activityLog);
            int counter = 1;
            foreach(var line in activity)
            {
                var log = RequestLog.FromCsv(line);

                if (log.Method == "GET")
                {
                    var swimbaitResponse = UriService.GetResponse("192.168.1.3", log.ActualPort, log.PathAndQuery);
                    var yamahaResponse = UriService.GetResponse("192.168.1.213", log.YamahaPort, log.PathAndQuery);
                    
                    var logService = new LogService();
                    logService.LogToDisk(counter, swimbaitResponse);
                    logService.LogToDisk(counter, yamahaResponse);
                }

                counter++;
            }
        }

        public static int MapPortToReal(Uri thisRequest)
        {
            // remap the port since windows is using 49154
            const int realYamahaPort = 49154;
            var relayPort = thisRequest.Port == 51123 ? realYamahaPort : thisRequest.Port;
            return relayPort;
        }
    }
}
