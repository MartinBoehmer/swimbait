using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Responses
{
    public class DeviceInfoResponse
    {
        public int response_code { get; set; }
        public string model_name { get; set; }
        public string destination { get; set; }
        public string system_id { get; set; }
        public decimal system_version { get; set; }
        public decimal api_version { get; set; }
        public string netmodule_version { get; set; }
        public string netmodule_checksum { get; set; }
        public string operation_mode { get; set; }
        public string update_error_code { get; set; }
    }
}
