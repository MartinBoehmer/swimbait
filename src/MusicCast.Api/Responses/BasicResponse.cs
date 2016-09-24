namespace MusicCast.Responses
{
    public class BasicResponse
    {
        public int response_code { get; set; }

        public BasicResponse()
        {
            response_code = 0;
        }
        public override string ToString()
        {
            return $"response_code={response_code}";
        }
    }
}
