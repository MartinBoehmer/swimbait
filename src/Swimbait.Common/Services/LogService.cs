using System.IO;
using System.Text;
using Swimbait.Common.Services;

namespace Swimbait.Common
{
    public class LogService
    {
        private readonly IEnvironmentService _environmentService;

        public LogService(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public void LogToDisk(int sequence, ResponseLog log)
        {
            var debugFolder = Path.Combine(_environmentService.ReplayLogFolder, @"log2Disk");
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
