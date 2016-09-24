using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Server;
using MusicCast.Responses;
using Swimbait.Server.Services;

namespace Swimbait.Server.Controllers
{
    public class BaseController : Controller
    {
        protected ILogger Log { get; private set; }

        public BaseController(ILoggerFactory loggerFactory)
        {
            Log = loggerFactory.CreateLogger<BaseController>();
        }

        public ObjectResult MusicCastOk()
        {
            return new ObjectResult(new BasicResponse());
        }

        public string ReadBody()
        {
            //var text = StreamService.ReadText(Request.Body);
            //return text;

            var prebody = Request.Body;
            Request.EnableRewind();
            var body = Request.Body;
            string result = string.Empty;
            if (Request.ContentLength != null && Request.ContentLength > 0)
            {
                byte[] buffer = new byte[Request.ContentLength.Value];
                body.ReadAsync(buffer, 0, (int)Request.ContentLength.Value).Wait();
                result = System.Text.Encoding.UTF8.GetString(buffer);
                
            }
            // Add this
            Request.Body = prebody;
            return result;
        }
    }
}
