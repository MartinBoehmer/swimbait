using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Swimbait.Server
{
    /// <summary>
    /// Fluent extension
    /// </summary>
    public static class AppContextExtensions
    {
        public static IApplicationBuilder UseHeadersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeadersMiddleware>();
        }
    }

    public class HeadersMiddleware
    {
        private readonly RequestDelegate next;

        public HeadersMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers["Server"] = "Network_Module/1.0 (WX-030)";
            return next(context);
        }
    }
}
