namespace Swimbait.Server.Controllers.Responses
{
    public class ZoneList
    {
        public bool main { get; set; }

        public ZoneList()
        {
            main = true;
        }
    }

    public class LocationInfoResponse
    {
        public int response_code { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string stereo_pair_status { get; set; }
        public ZoneList zone_list { get; set; }

        public LocationInfoResponse()
        {
            response_code = 0;
            stereo_pair_status = "none";
            name = "Home";
            zone_list = new ZoneList();
        }
    }
}
