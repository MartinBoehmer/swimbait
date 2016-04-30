using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Swimbait.Server.Controllers.Responses;

namespace Swimbait.Server.Controllers
{
   

    [Route("MusicCastNetwork")]
    public class MusicCastNetworkController : Controller
    {
        public MusicCastNetworkController()
        {

        }
        
        [HttpGet("InitialJoinComplete")]
        public IActionResult InitialJoinComplete()
        {
            var response = new BasicResponse();
            return new ObjectResult(response);
        }

    }

}
