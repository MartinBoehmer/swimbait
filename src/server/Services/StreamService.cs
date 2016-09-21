using System.IO;

namespace Swimbait.Server.Services
{
    public class StreamService
    {
        public static MemoryStream FromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
