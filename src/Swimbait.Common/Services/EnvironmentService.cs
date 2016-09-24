using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Swimbait.Common.Services
{
    public interface IEnvironmentService
    {
        string IpAddress { get;  }

        string SubnetBroadcastIp { get; }
    }

    public class EnvironmentService : IEnvironmentService
    {
        public const int YamahaDlnaPort = 49154;
        
        public const int SwimbaitDlnaPort = 51123;

        public string IpAddress { get; set; }

        public string SubnetBroadcastIp { get; set; }

        public EnvironmentService()
        {
            var ipHostEntry = Dns.GetHostEntryAsync(Dns.GetHostName()).Result;
            IpAddress = ipHostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();

            SubnetBroadcastIp = "192.168.1.255";
        }
    }
}
