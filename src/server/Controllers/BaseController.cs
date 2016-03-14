using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

namespace Swimbait.Server.Controllers
{
    public class BaseController : Controller
    {
        protected ILogger Log { get; private set; }

        public BaseController(ILoggerFactory loggerFactory)
        {
            Log = loggerFactory.CreateLogger<BaseController>();
        }
    }
}
