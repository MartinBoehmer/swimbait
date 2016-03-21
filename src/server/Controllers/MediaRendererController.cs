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
        private MusicCastHost _host;

        public MediaRendererController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _host = new MusicCastHost();
        }

        [Route("desc.xml")]
        //[Produces("application/xml")]
        [HttpGet]
        public IActionResult GetDescription()
        {
            var response = new MediaDescriptionResponse();
            response.IpAddress = _host.IpAddress;
            response.Uuid = _host.Uuid;
            response.FriendlyName = _host.Name;
            response.SerialNumber = _host.SerialNumber;
            //Response.ContentType = "text/xml;";
            //var result = Ok(response.GetXml());
            //result.ContentTypes.Add(new MediaTypeHeaderValue("text/xml"));
            return response;
        }
    }
}
