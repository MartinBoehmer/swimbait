using System.Collections.Generic;
using System.Linq;

namespace YamahaExtendedControl.Responses
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
                            .Select(i => new PresetInfo {input = "unknown"}).ToList();
        }
    }
}
