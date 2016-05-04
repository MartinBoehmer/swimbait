using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Swimbait.Server.Multicast;
using Swimbait.Server.Multicast.Requests;

namespace Swimbait.Server.Services
{
    public class MulticastService
    {
        const string controllerIp = "192.168.1.181";
        private MulticastSender _sender;
        public MulticastService()
        {
            _sender = new MulticastSender();
        }

        public void SendNotSureWhatThisDoesUdp()
        {
            var message = new MulticastRequest("{\"main\":{\"power\":\"on\"}}{\"netusb\":{\"play_info_updated\":true}}{\"system\":{\"location_info_updated\":true,\"stereo_pair_info_updated\":true},\"netusb\":{\"account_updated\":true,\"play_info_updated\":true}}{\"system\":{\"stereo_pair_info_updated\":true}}{\"system\":{\"location_info_updated\":true},\"netusb\":{\"play_info_updated\":true}}{\"system\":{\"name_text_updated\":true}}");
            
            IPEndPoint RemoteEndPoint = new IPEndPoint(
            IPAddress.Parse(controllerIp), 41100);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            var bytes = message.AsBytes();
            s.SendTo(bytes, bytes.Length, SocketFlags.None, RemoteEndPoint);
        }

        public void SendConnectUdp()
        {
            var data = "{\"location\":\"http://!!IP!!:!!DlnaHostPort!!/MediaRenderer/desc.xml\",\"ack\":\"http://!!IP!!:!!DlnaHostPort!!/MusicCastNetwork/InitialJoinComplete\"}".Replace("!!DlnaHostPort!!", MusicCastHost.DlnaHostPort.ToString());

            data = data
                     .Replace("!!IP!!", MusicCastHost.ThisIp)
                     .Replace("!!DlnaHostPort!!", MusicCastHost.DlnaHostPort.ToString());

            var message = new MulticastRequest(data);

            IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.255"), 51100);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var bytes = message.AsBytes();
            s.SendTo(bytes, bytes.Length, SocketFlags.None, RemoteEndPoint);
        }
    }
}
