using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Responses
{
    public class BasicResponse
    {
        public int response_code { get; set; }

        public BasicResponse()
        {
            response_code = 0;
        }
    }
}
