using System.Collections.Generic;

namespace YamahaExtendedControl.Responses
{
    public class DistributionInfoResponse
    {
        public int response_code { get; set; }
        public string group_id { get; set; }
        public string group_name { get; set; }
        public string role { get; set; }
        public IList<object> client_list { get; set; }

        public DistributionInfoResponse()
        {
            client_list = new List<object>();
        }
    }
}
