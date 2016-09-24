using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Swimbait.Server.Multicast.Requests;
using Swimbait.Common.Services;

namespace Swimbait.Server.Multicast
{
    public class MulticastServer : IDisposable
    {
        private readonly IEnvironmentService _environmentService;
        private Socket _udpSocket;
        bool _disposed = false;
        const int _endpointPort = 1900;
        const int _sourcePort = 44075;
        const string _ssdpMulticastIp = "239.255.255.250";
        const string multiCastEndpoint = "224.0.0.22";
        private IPEndPoint _multicastEndPoint;

        public MulticastServer(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public Task Start()
        {
            var task = new Task(SynchronousStart);
            task.Start();
            return task;
        }

        public void SsdpDiscover()
        {
            var message = new SsdpDiscover(_ssdpMulticastIp, _endpointPort);
            Send(message);
            Console.WriteLine("M-Search sent...\r\n");
        }

        public void JoinGroup()
        {
            var multicastIp = IPAddress.Parse(multiCastEndpoint);
            var sourceIp = IPAddress.Parse(_environmentService.IpAddress);
            var localIp = IPAddress.Parse("0.0.0.0");

            byte[] membershipAddresses = new byte[12]; // 3 IPs * 4 bytes (IPv4)
            Buffer.BlockCopy(multicastIp.GetAddressBytes(), 0, membershipAddresses, 0, 4);
            Buffer.BlockCopy(sourceIp.GetAddressBytes(), 0, membershipAddresses, 4, 4);
            Buffer.BlockCopy(localIp.GetAddressBytes(), 0, membershipAddresses, 8, 4);

            //          var bytesTest = new byte[]{0x22, 0x00, 0xeb, 0x03, 0x00, 0x00, 0x00, 0x01, 0x03, 0x00, 0x00, 0x00, 0xef, 0xff, 0xff, 0xfa};
            //            _udpSocket.SendTo(bytesTest, SocketFlags.None, _multicastEndPoint);

            _udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddSourceMembership, membershipAddresses);

            //var option = new MulticastOption(multicastIp);
            //_udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, option);
        }

        private void Send(IMulticastRequest request)
        {
            _udpSocket.SendTo(request.AsBytes(), SocketFlags.None, _multicastEndPoint);
        }

        private void SynchronousStart()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, _sourcePort);

             var multicastIp = IPAddress.Parse(_ssdpMulticastIp);
            _multicastEndPoint = new IPEndPoint(multicastIp, _endpointPort);

            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var option = new MulticastOption(multicastIp);
            _udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, option);
            _udpSocket.Bind(localEndPoint);

            Console.WriteLine("UDP-Socket setup done...\r\n");

            SsdpDiscover();

            byte[] receiveBuffer = new byte[64000];

            while (true)
            {
                if (_udpSocket.Available > 0)
                {
                    var receivedBytes = _udpSocket.Receive(receiveBuffer, SocketFlags.None);

                    if (receivedBytes > 0)
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(receiveBuffer, 0, receivedBytes));
                    }


                    //Task.Delay(50).ConfigureAwait(false);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                //_udpSocket.Close();
                _udpSocket.Shutdown(SocketShutdown.Both);
                _udpSocket.Dispose();
            }
            
            _disposed = true;
        }
    }
}
