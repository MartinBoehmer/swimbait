namespace Swimbait.Server.Controllers.Responses
{
    public class Audio
    {
        public int error { get; set; }
        public string format { get; set; }
        public string fs { get; set; }
    }

    public class SignalInfoResponse : BasicResponse
    {
        public Audio audio { get; set; }

        public SignalInfoResponse()
        {
            audio = new Audio();
        }
    }

}
