using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swimbait.Server.Controllers.Responses;
using Swimbait.Server.Services;

namespace Swimbait.Server.Controllers
{
    [Route("MediaRenderer")]
    public class MediaRendererController : BaseController
    {
        private readonly MusicCastHost _musicCastHost;

        public MediaRendererController(
            ILoggerFactory loggerFactory
            , MusicCastHost musicCastHost) : base(loggerFactory)
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
