using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace swimbait
{
    class Program
    {
        static void Main(string[] args)
        {

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 60000);

            var endpointPort = 1900;
            var endpointIp = "239.255.255.250";
            IPEndPoint multicastEndPoint = new IPEndPoint(IPAddress.Parse(endpointIp), endpointPort);

            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

          //  udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpSocket.Bind(localEndPoint);
            //udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastEndPoint.Address, IPAddress.Any));
            //udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
            //udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);

            Console.WriteLine("UDP-Socket setup done...\r\n");

            string searchString = $@"M-SEARCH * HTTP/1.1
Host: {endpointIp}:{endpointPort}
Content-Length: 0
MAN: ""ssdp:discover""
MX: 2
ST: urn:schemas-upnp-org:device:MediaServer:1

";

            udpSocket.SendTo(Encoding.UTF8.GetBytes(searchString), SocketFlags.None, multicastEndPoint);

            Console.WriteLine("M-Search sent...\r\n");

            byte[] receiveBuffer = new byte[64000];

            while (true)
            {
                if (udpSocket.Available > 0)
                {
                    var receivedBytes = udpSocket.Receive(receiveBuffer, SocketFlags.None);

                    if (receivedBytes > 0)
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(receiveBuffer, 0, receivedBytes));
                    }
                }
            }
        }
    }
}
