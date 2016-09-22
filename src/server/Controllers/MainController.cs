using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swimbait.Server.Services;
using YamahaExtendedControl.Responses;

namespace Swimbait.Server.Controllers
{
    [Route("YamahaExtendedControl/v1/main")]
    public class MainController : BaseController
    {
        private MusicCastHost _musicCastHost;

        public MainController(ILoggerFactory loggerFactory, MusicCastHost musicCastHost) : base(loggerFactory)
        {
            _musicCastHost = musicCastHost;
        }
        
        /// <param name="power">standby</param>
        [HttpGet("setInput")]
        public IActionResult setPower(string power)
        {
            var response = new BasicResponse();
            return new ObjectResult(response);
        }

        [HttpGet("setInput")]
        public IActionResult setInput(string input)
        {
            var response = new BasicResponse();
            return new ObjectResult(response);
        }
        
        [HttpGet("prepareInputChange")]
        public IActionResult PrepareInputChange(string input)
        {
            var response = new BasicResponse();
            return new ObjectResult(response);
        }

        [HttpGet("getSignalInfo")]
        public IActionResult GetSignalInfo()
        {
            var response = new SignalInfoResponse();
            return new ObjectResult(response);
        }

        [HttpGet("getStatus")]
        public IActionResult GetStatus()
        {
            var response = new StatusResponse();
            response.response_code = 0;
            response.power = "on";
            response.sleep = 0;
            response.volume = 30;
            response.mute = false;
            response.max_volume = 60;
            response.input = "mc_link";
            response.distribution_enable = false;
            response.equalizer = new Equalizer {low =0, mid=0, high =0};
            response.link_control = "standard";
            response.disable_flags = 0;
            return new ObjectResult(response);
        }
    }
}
