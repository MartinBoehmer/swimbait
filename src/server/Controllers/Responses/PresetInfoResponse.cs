using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Responses
{
    public class PresetInfo
    {
        public string input { get; set; }
        public string text { get; set; }
    }

    public class PresetInfoResponse
    {
        public int response_code { get; set; }

        public List<PresetInfo> preset_info { get; set; }

        public PresetInfoResponse()
        {
            preset_info = Enumerable
                            .Range(1, 40)
                            .ToList()
                            .ConvertAll(i => new PresetInfo {input = "unknown"});
        }
    }
}
