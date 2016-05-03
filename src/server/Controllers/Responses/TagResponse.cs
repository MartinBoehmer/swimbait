using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Requests
{
    public static class InputListExtensions
    {
        public static void Add(this List<IntegerInputList> target, string id, int tag)
        {
            target.Add(new IntegerInputList { id = id, tag = tag });
        }

        public static void Add(this IList<InputList> target, string id, string tag)
        {
            target.Add(new InputList { id = id, tag = tag });
        }

        public static void AddText(this IList<InputList3> target, string id, string text)
        {
            target.Add(new InputList3 { id = id, text = text });
        }
    }

    public class IntegerInputList
    {
        public string id { get; set; }
        public int tag { get; set; }

    }

    public class InputList
    {
        public string id { get; set; }
        public string tag { get; set; }
    }

    public class InputList3
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class GetTagResponse
    {
        public int response_code { get; set; }
        public List<IntegerInputList> zone_list { get; set; }
        public List<IntegerInputList> input_list { get; set; }

        public GetTagResponse()
        {
            zone_list = new List<IntegerInputList>();
            input_list = new List<IntegerInputList>();
        }

      
    }
}
