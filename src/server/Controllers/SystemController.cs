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
using Swimbait.Server.Controllers.Responses;

namespace Swimbait.Server.Controllers
{
    [Route("YamahaExtendedControl/v1/system")]
    public class SystemController : BaseController
    {
        public SystemController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {

        }

        [Route("GetFeatures")]
        [HttpGet]
        public IActionResult GetFeatures()
        {
            Log.LogInformation("Features!");
            var response = new FeaturesResponse();
            return new ObjectResult(response);
        }
    }
}
