using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using swimbait_srv2.Multicast.Requests;

namespace swimbait_srv2.Multicast
{
    public class MulticastServer : IDisposable
    {
        private Socket _udpSocket;
        bool _disposed = false;
        const int _endpointPort = 1900;
        const string _endpointIp = "239.255.255.250";
        private IPEndPoint _multicastEndPoint;

        public Task Start()
        {
            var task = new Task(SynchronousStart);
            task.Start();
            return task;
        }

        public void SsdpDiscover()
        {
            var message = new SsdpDiscover(_endpointIp, _endpointPort);
            Send(message);
            Console.WriteLine("M-Search sent...\r\n");
        }

        private void Send(IMulticastRequest request)
        {
            _udpSocket.SendTo(request.AsBytes(), SocketFlags.None, _multicastEndPoint);
        }

        private void SynchronousStart()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 60000);

            _multicastEndPoint = new IPEndPoint(IPAddress.Parse(_endpointIp), _endpointPort);

            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //  udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpSocket.Bind(localEndPoint);
            //udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastEndPoint.Address, IPAddress.Any));
            //udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
            //udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);

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
                _udpSocket.Close();
                _udpSocket.Dispose();
            }
            
            _disposed = true;
        }
    }
}
