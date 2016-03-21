using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Requests
{
    public class TagZoneList
    {
        public string id { get; set; }
        public int tag { get; set; }
    }

    public class InputList
    {
        public string id { get; set; }
        public int tag { get; set; }
    }

    public class GetTagResponse
    {
        public int response_code { get; set; }
        public List<TagZoneList> zone_list { get; set; }
        public List<InputList> input_list { get; set; }

        public GetTagResponse()
        {
            zone_list = new List<TagZoneList>();
            input_list = new List<InputList>();
        }

        public void AddInputList(string id, int tag = 0)
        {
            input_list.Add(new InputList {id=id,tag=tag});
        }
    }
}
