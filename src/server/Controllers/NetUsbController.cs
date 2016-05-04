using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Swimbait.Server.Controllers.Responses;
using Swimbait.Server.Services;

namespace Swimbait.Server.Controllers
{
    [Route("YamahaExtendedControl/v1/NetUsb")]
    public class NetUsbController : BaseController
    {
        private MusicCastHost _musicCastHost;

        public NetUsbController(ILoggerFactory loggerFactory, MusicCastHost musicCastHost) : base(loggerFactory)
        {
            _musicCastHost = musicCastHost;
        }


        [HttpGet("getPresetInfo")]
        public IActionResult getPresetInfo()
        {
            var response = new PresetInfoResponse();
            return new ObjectResult(response);
        }

        [HttpGet("getPlayInfo")]
        public IActionResult GetPlayInfo()
        {
            var response = new PlayInfoResponse();
            response.response_code = 0;
            response.input = "mc_link";
            response.playback = "stop"; // play,stop
            response.repeat = "off";
            response.shuffle = "off";
            response.artist= "";
            response.album= "";
            response.track= "";
            response.albumart_url = "";
            response.albumart_id = 16;
            response.play_time = 0;
            response.usb_devicetype= "unknown";
            response.auto_stopped= false;
            response.attribute= 0;

            return new ObjectResult(response);
        }
    }
}
