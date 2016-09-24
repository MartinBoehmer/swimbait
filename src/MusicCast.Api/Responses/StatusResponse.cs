namespace MusicCast.Responses
{
    public class Equalizer
    {
        public int low { get; set; }
        public int mid { get; set; }
        public int high { get; set; }
    }

    public class StatusResponse
    {
        public int response_code { get; set; }
        public string power { get; set; }
        public int sleep { get; set; }
        public int volume { get; set; }
        public bool mute { get; set; }
        public int max_volume { get; set; }
        public string input { get; set; }
        public bool distribution_enable { get; set; }
        public Equalizer equalizer { get; set; }
        public string link_control { get; set; }
        public int disable_flags { get; set; }
    }
}
