using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Responses
{
    public class NameTextZoneList
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class InputList
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class NameTextResponse
    {
        public int response_code { get; set; }
        public IList<NameTextZoneList> zone_list { get; set; }
        public IList<InputList> input_list { get; set; }
        public IList<object> sound_program_list { get; set; }
    }
}
