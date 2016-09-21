using System.IO;
using System.Text;

namespace Swimbait.Common
{
    public class LogService
    {
        public void LogToDisk(int sequence, ResponseLog log)
        {
            var debugFolder = @"D:\Downloads\swimbait\log2Disk";
            Directory.CreateDirectory(debugFolder);

            var pathAsSafeFilename = log.RequestUri.PathAndQuery.Replace("/", "_").Replace("?", "_");
            var filename = sequence + "_" + pathAsSafeFilename + "_" + log.RequestUri.Host;

            var debugFile = Path.Combine(debugFolder, $"{filename}.txt");

            var sb = new StringBuilder();
            sb.AppendLine($"Request.Url={log.RequestUri}");
            sb.AppendLine($"Request.Body:{log.RequestBody}");
            sb.AppendLine($"Request.Method:{log.RequestMethod}");
            sb.AppendLine("Response:");
            sb.Append(log.ResponseBody);

            File.WriteAllText(debugFile, sb.ToString());
        }
    }
}
