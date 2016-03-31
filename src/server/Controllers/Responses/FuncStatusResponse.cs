using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Responses
{
    public class FuncStatusResponse
    {
        public int response_code { get; set; }
        public bool auto_power_standby { get; set; }
    }
}
