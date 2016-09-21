using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swimbait.Server.Controllers.Responses;

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
    }
}
