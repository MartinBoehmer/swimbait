using System.Collections.Generic;

namespace YamahaExtendedControl.Responses
{
    public class NameTextResponse2 : BasicResponse
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class NameTextResponse : BasicResponse
    {
        public IList<InputList3> zone_list { get; set; }
        public IList<InputList3> input_list { get; set; }
        public IList<object> sound_program_list { get; set; }

        public NameTextResponse()
        {
            zone_list = new List<InputList3>();
            input_list = new List<InputList3>();
            sound_program_list = new List<object>();
        }
    }
}
