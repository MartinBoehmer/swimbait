using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Swimbait.Common.Services
{
    public interface IEnvironmentService
    {
        IPAddress IpAddress { get; }

        IPAddress SubnetBroadcastIp { get; }

        /// <summary>
        /// For debug purposes, logging and replaying requests to the swimbait server
        /// </summary>
        string LogFolderReplayRoot { get; set; }

        string ActivityLogFilename { get; }
    }

    public class EnvironmentService : IEnvironmentService
    {
        public const int YamahaDlnaPort = 49154;
        
        public const int SwimbaitDlnaPort = 51123;

        public IPAddress IpAddress { get; set; }

        public IPAddress SubnetBroadcastIp { get; set; }

        public string LogFolderReplayRoot { get; set; }

        public string ActivityLogFilename => Path.Combine(LogFolderReplayRoot, "activity.txt");

        public EnvironmentService()
        {
            var ipHostEntry = Dns.GetHostEntryAsync(Dns.GetHostName()).Result;
            IpAddress = ipHostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            
            SubnetBroadcastIp = GetBroadcastIP();

            //todo: read from configuration
            LogFolderReplayRoot = @"D:\Downloads\Swimbait";
        }

        /// <summary>
        /// http://www.java2s.com/Code/CSharp/Network/GetSubnetMask.htm
        /// </summary>
        private static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }

        /// <summary>
        /// http://stackoverflow.com/questions/18551686/how-do-you-get-hosts-broadcast-address-of-the-default-network-adapter-c-sharp
        /// </summary>
        private IPAddress GetBroadcastIP()
        {
            IPAddress maskIP = GetSubnetMask(IpAddress);
            IPAddress hostIP = IpAddress;

            if (maskIP == null || hostIP == null)
                return null;

            byte[] complementedMaskBytes = new byte[4];
            byte[] broadcastIPBytes = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                complementedMaskBytes[i] = (byte)~(maskIP.GetAddressBytes().ElementAt(i));
                broadcastIPBytes[i] = (byte)((hostIP.GetAddressBytes().ElementAt(i)) | complementedMaskBytes[i]);
            }

            return new IPAddress(broadcastIPBytes);

        }
    }
}
