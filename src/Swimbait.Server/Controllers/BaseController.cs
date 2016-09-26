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

        /// <summary>
        /// The iOS app POSTs the data application/formurlencoded and not application/json which basically stuffs model binding
        /// </summary>
        public string ReadBody()
        {
            return StreamService.ReadText(Request.Body);
        }
    }
}
