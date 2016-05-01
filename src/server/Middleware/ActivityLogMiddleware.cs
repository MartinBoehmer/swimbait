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
using System.Text;

namespace Swimbait.Server
{
    /// <summary>
    /// Fluent extension
    /// </summary>
    public static class ActivityLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseActivityLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ActivityLogMiddleware>();
        }
    }

    public class ActivityLogMiddleware
    {
        private readonly RequestDelegate next;

        public ActivityLogMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var uri = request.GetUri();
            var path = uri.PathAndQuery;

            var debugFolder = @"D:\Downloads\swimbait";
            Directory.CreateDirectory(debugFolder);
            var filename = Path.Combine(debugFolder, "activity.txt");
            
            var thisPort = uri.Port;
            var yamahaPort = LogMiddleware.MapPortToReal(uri);

            var body = "<not decoded>";
            if (!uri.ToString().Contains("secure") && request.Method.ToLower() == "post")
            {
                body = request.Form.Keys.First();
            }

            var lineContent = $"{thisPort},{yamahaPort},{request.Method},{path},{body},{Environment.NewLine}";

            File.AppendAllText(filename, lineContent);

            return next(context);
        }
    }
}
