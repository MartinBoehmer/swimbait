using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swimbait.Server.Services;
using Swimbait.Common;

namespace Swimbait.Server.Controllers
{
    [Route("YamahaExtendedControl/secure/v1/netusb")]
    public class SecureController : BaseController
    {
        public SecureController(ILoggerFactory logFactory) :base(logFactory)
        {
         
        }

        [HttpGet("getAccountStatus")]
        public IActionResult GetAccountStatus()
        {
            return DoManInTheMiddle();
        }

        [Route("YamahaExtendedControl/secure/v1/system")]
        [HttpGet("getNetworkStatus")]
        public IActionResult GetNetworkStatus()
        {
            return DoManInTheMiddle();
        }

        private IActionResult DoManInTheMiddle()
        {

            var uri = HttpContext.Request.GetUri();
            var manInTheMiddleResponse = UriService.GetManInTheMiddleResult(MusicCastHost.RelayHost, uri, ActivityLogMiddleware.MapPortToReal);
            return new FileContentResult(manInTheMiddleResponse.ResponseBytes, "application/octet-stream");
        }
    }
}
