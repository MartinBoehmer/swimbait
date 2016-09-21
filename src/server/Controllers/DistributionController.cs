using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swimbait.Server.Controllers.Responses;
using Swimbait.Server.Services;

namespace Swimbait.Server.Controllers
{
    [Route("YamahaExtendedControl/v1/dist")]
    public class DisttributionController : BaseController
    {
        private MusicCastHost _musicCastHost;

        public DisttributionController(ILoggerFactory loggerFactory, MusicCastHost musicCastHost) : base(loggerFactory)
        {
            _musicCastHost = musicCastHost;
        }
        
        [HttpGet("getDistributionInfo")]
        public IActionResult GetDistributionInfo()
        {
            var response = new DistributionInfoResponse();

            response.response_code = 0;
            response.group_id = "00000000000000000000000000000000";
            response.group_name = _musicCastHost.Name;
            response.role = "none";
            
            return new ObjectResult(response);
        }
    }
}
