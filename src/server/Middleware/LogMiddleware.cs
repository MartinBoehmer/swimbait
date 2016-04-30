using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System.Net.Http;
using Swimbait.Server.Services;
using Microsoft.ApplicationInsights.AspNet.Extensions;
using System.IO;
using Swimbait.Server.Services;

namespace Swimbait.Server
{
    /// <summary>
    /// Fluent extension
    /// </summary>
    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }

    public class LogMiddleware
    {
        private readonly RequestDelegate next;

        public LogMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var uri = context.Request.GetUri();

            var body = "<not decoded>";
            if (!uri.ToString().Contains("secure") && context.Request.Method.ToLower() == "post")
            {
                body = context.Request.Form.Keys.First();
            }

            var thisResponseLog = new ResponseLog();
            thisResponseLog.RequestUri = uri;
            thisResponseLog.RequestBody = body;
            thisResponseLog.ResponseBody = context.Response.ToString();
                
            var manInTheMiddleResult = GetManInTheMiddleResult(uri);

            var logService = new LogService();
            logService.LogToDisk(thisResponseLog);
            logService.LogToDisk(manInTheMiddleResult);

            return next(context);
        }

        public static ResponseLog GetManInTheMiddleResult(Uri thisRequest)
        {
            var result = new ResponseLog();
            using (var httpClient = new HttpClient())
            {
                // remap the port since windows is using 49154
                var relayPort = thisRequest.Port == MusicCastHost.DlnaHostPort ? 49154 : thisRequest.Port;
                var relayUri = new Uri($"http://{MusicCastHost.RelayHost}:{relayPort}" + thisRequest.PathAndQuery);

                result.RequestUri = relayUri;
                result.ResponseBody = httpClient.GetStringAsync(relayUri).Result;
            }
            return result;
        }
    }
}
