using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Swimbait.Server.Services
{
    public class MusicCastHost
    {
        private Dictionary<string, string> _tags;

        public const int DlnaHostPort = 51123;

        public static string ThisIp { get; set; }

        public const string RelayHost = "192.168.1.213";

        public string LocationId { get; set; }

        public string IpAddress => "192.168.1.3";

        public string Uuid => "uuid:9ab0c000-f668-11de-9976-00a0ded0e860";

        public string SerialNumber => "0F437184";

        public decimal SystemVersion => 2.09m;

        public decimal ApiVersion => 1.11m;

        public string Name { get; set; }


        public MusicCastHost()
        {
            Name = Environment.MachineName;
            _tags = new Dictionary<string, string>();
        }

        static MusicCastHost()
        {
            Dns.GetHostEntryAsync(Dns.GetHostName()).ContinueWith(
                   r => ThisIp = r.Result.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString()
            );
        }

        public bool HasTag(string key)
        {
            return _tags.ContainsKey(key);
        }

        public string GetTag(string key)
        {
            if (HasTag(key)) {
                return _tags[key];
            }
            return null;
        }

        public void SetTag(string key, string value)
        {
            if (_tags.ContainsKey(key)) {
                _tags[key] = value;
            } else {
                if (value != null) {
                    _tags.Add(key, value);
                }
            }
        }
    }
}