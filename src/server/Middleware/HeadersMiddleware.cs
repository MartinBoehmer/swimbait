using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Swimbait.Server
{
    /// <summary>
    /// Fluent extension
    /// </summary>
    public static class HeadersMiddlewareExtensions
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
