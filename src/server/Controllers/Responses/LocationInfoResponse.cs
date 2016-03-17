using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Responses
{
    public class ZoneList
    {
        public bool main { get; set; }
    }

    public class LocationInfoResponse
    {
        public int response_code { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string stereo_pair_status { get; set; }
        public ZoneList zone_list { get; set; }
    }
}
