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
using Swimbait.Server.Controllers.Requests;
using Swimbait.Server.Controllers.Responses;
using Swimbait.Server.Services;

namespace Swimbait.Server.Controllers
{
    [Route("YamahaExtendedControl/v1/system")]
    public class SystemController : BaseController
    {
        private MusicCastHost _musicCastHost;

        public SystemController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _musicCastHost = new MusicCastHost();
        }
        
        [HttpGet("GetFeatures")]
        public IActionResult GetFeatures()
        {
            var response = new FeaturesResponse();
            return new ObjectResult(response);
        }
        
        [HttpGet("getLocationInfo")]
        public IActionResult GetLocationInfo()
        {
            var response = new LocationInfoResponse();
            response.id = _musicCastHost.LocationId;
            return new ObjectResult(response);
        }
        
        [HttpPost("SetLocationName")]
        public IActionResult SetLocationName([FromBody] SetLocationNameRequest request)
        {
            Log.LogInformation($"Set name={request.name}");
            return Ok();
        }
    }
}
