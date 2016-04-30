using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swimbait.Server.Services
{
    public class ResponseLog
    {
        public Uri RequestUri { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }
    }

    public class LogService
    {
        public void LogToDisk(ResponseLog log)
        {
            var debugFolder = @"D:\Downloads\swimbait\log2Disk";
            Directory.CreateDirectory(debugFolder);

            var filename = log.RequestUri.Host + "_" + log.RequestUri.PathAndQuery.Replace("/", "_").Replace("?", "_");

            var debugFile = Path.Combine(debugFolder, $"{filename}.txt");

            var sb = new StringBuilder();
            sb.AppendLine($"Request.Url={log.RequestUri.ToString()}");
            sb.AppendLine($"Request.Body:{log.RequestBody}");
            sb.Append(log.ResponseBody);

            File.WriteAllText(debugFile, sb.ToString());
        }
    }
}
