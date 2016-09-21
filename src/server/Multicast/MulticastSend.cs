using System.Net;
using System.Net.Sockets;
using Swimbait.Server.Multicast.Requests;

namespace Swimbait.Server.Multicast
{
    public class MulticastSender
    {
        //public void Send(string message)
        //{
            
        //}

        public void Send(string multicastGroup, int port, IMulticastRequest request)
        {
            var ip = IPAddress.Parse(multicastGroup);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));

            //s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, int.Parse(ttl));
            
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(multicastGroup), port);
            
            s.Connect(ipEndPoint);
            var bytes = request.AsBytes();
            s.Send(bytes, bytes.Length, SocketFlags.None);
            //s.Close();
            s.Shutdown(SocketShutdown.Both);
        }


    }
}
