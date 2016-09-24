using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Swimbait.Server.Services
{
    public class MusicCastHost
    {
        private readonly IEnvironmentService _environmentService;
        private Dictionary<string, string> _tags;

        public const int DlnaHostPort = 51123;
        
        /// <summary>
        /// Should be the IP of a real MusicCast speaker (read in via config?) so that man in the middle relays work
        /// 
        /// ie: If this server doesn't know how to handle a request, it relays it to the real musicCast speaker and replays that back to the client
        /// </summary>
        public const string RelayHost = "192.168.1.213";

        public string LocationId { get; set; }
        

        public string Uuid => "uuid:9ab0c000-f668-11de-9976-00a0ded0e860";

        public string SerialNumber => "0F437184";

        public decimal SystemVersion => 2.09m;

        public decimal ApiVersion => 1.11m;

        public string Name { get; set; }

        public string IpAddress => _environmentService.IpAddress;

        public MusicCastHost(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
            Name = Environment.MachineName;
            _tags = new Dictionary<string, string>();
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