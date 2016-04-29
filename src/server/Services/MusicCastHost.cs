using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Services
{
    public class MusicCastHost
    {
        public const int DlnaHostPort = 51123;

        public const string RelayHost = "192.168.1.213";

        public string LocationId => "7345174dcfd44df2834f1d02452cbe3c";

        public string IpAddress => "192.168.1.3";

        public string Uuid => "uuid:9ab0c000-f668-11de-9976-00a0ded0e860";

        public string SerialNumber => "0F437184";

        public decimal SystemVersion => 2.09m;

        public decimal ApiVersion => 1.11m;
        
        public string Name { get; set; }

        public MusicCastHost()
        {
            Name = "Swimbait";
        }
    }
}