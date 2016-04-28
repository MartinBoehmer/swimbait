using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Swimbait.Server.Controllers.Responses;
using Swimbait.Server.Services;

namespace Swimbait.Server.Controllers
{
    [Route("MediaRenderer")]
    public class MediaRendererController : BaseController
    {
        private MusicCastHost _musicCastHost;

        public MediaRendererController(ILoggerFactory loggerFactory, MusicCastHost musicCastHost) : base(loggerFactory)
        {
            _musicCastHost = musicCastHost;
        }

        [Route("desc.xml")]
        [HttpGet]
        public IActionResult GetDescription()
        {
            var response = new MediaDescriptionResponse();
            response.IpAddress = _musicCastHost.IpAddress;
            response.Uuid = _musicCastHost.Uuid;
            response.FriendlyName = _musicCastHost.Name;
            response.SerialNumber = _musicCastHost.SerialNumber;
            return response;
        }
    }
}
