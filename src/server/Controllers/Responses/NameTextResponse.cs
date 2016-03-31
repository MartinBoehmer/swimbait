using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swimbait.Server.Controllers.Requests;

namespace Swimbait.Server.Controllers.Responses
{

    public class NameTextResponse
    {
        public int response_code { get; set; }
        public IList<InputList> zone_list { get; set; }
        public IList<InputList> input_list { get; set; }
        public IList<object> sound_program_list { get; set; }
    }
}
