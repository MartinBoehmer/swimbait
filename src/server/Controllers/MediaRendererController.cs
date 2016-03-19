using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Swimbait.Server.Controllers.Responses;

namespace Swimbait.Server.Controllers
{
    [Route("MediaRenderer")]
    public class MediaRendererController : BaseController
    {
        public MediaRendererController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        [Route("desc.xml")]
        [HttpGet]
        public IActionResult GetDescription()
        {
            var response = new MediaDescriptionResponse();

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new XmlContent(response)
            };
        }
    }
}
