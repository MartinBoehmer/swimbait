/*
 * Replaced with ActivityLogMiddlware
 * 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Swimbait.Server.Services;
using System.IO;
using System.Text;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Swimbait.Common;

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
            context.Response.OnCompleted(OnCompleted, context);
            return next(context);
        }


        private Task OnCompleted(object contextObject)
        {
            var context = (HttpContext) contextObject;

            var uri = context.Request.GetUri();

            var body = "<not decoded>";
            if (!uri.ToString().Contains("secure") && context.Request.Method.ToLower() == "post")
            {
                body = context.Request.Form.Keys.First();
            }

            var thisResponseLog = new ResponseLog();
            thisResponseLog.RequestUri = uri;
            thisResponseLog.RequestBody = body;
            //var bodyStream = context.Response.Body;

            //using (var reader = new StreamReader(bodyStream, Encoding.UTF8))
            //{
            //    bodyStream.Seek(0, SeekOrigin.Begin);
            //    thisResponseLog.ResponseBody = reader.ReadToEnd();
            //}
            
            var manInTheMiddleResult = UriService.GetManInTheMiddleResult(uri, ActivityLogMiddleware.MapPortToReal);

            var logService = new LogService();
            logService.LogToDisk(thisResponseLog);
            logService.LogToDisk(manInTheMiddleResult);

            return Task.FromResult(0);
        }
    }
}
*/
