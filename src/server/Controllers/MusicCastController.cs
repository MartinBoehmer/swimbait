using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Swimbait.Server.Controllers
{
   

    [Route("api/MusicCast")]
    public class MusicCastController : Controller
    {
        public MusicCastController()
        {

        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }

}
