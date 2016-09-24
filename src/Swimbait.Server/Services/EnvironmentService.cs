using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Swimbait.Server.Services
{
    public interface IEnvironmentService
    {
        string IpAddress { get;  }

        string SubnetBroadcastIp { get; }
    }

    public class EnvironmentService : IEnvironmentService
    {
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
