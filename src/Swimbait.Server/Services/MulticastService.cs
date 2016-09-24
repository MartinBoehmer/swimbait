using System.Net;
using System.Net.Sockets;
using Swimbait.Server.Multicast;
using Swimbait.Server.Multicast.Requests;
using Swimbait.Common.Services;

namespace Swimbait.Server.Services
{
    public class MulticastService
    {
        private readonly IEnvironmentService _environmentService;
        private MulticastSender _sender;

        public MulticastService(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
            _sender = new MulticastSender();
        }

        public void SendNotSureWhatThisDoesUdp()
        {
            var message = new MulticastRequest("{\"main\":{\"power\":\"on\"}}{\"netusb\":{\"play_info_updated\":true}}{\"system\":{\"location_info_updated\":true,\"stereo_pair_info_updated\":true},\"netusb\":{\"account_updated\":true,\"play_info_updated\":true}}{\"system\":{\"stereo_pair_info_updated\":true}}{\"system\":{\"location_info_updated\":true},\"netusb\":{\"play_info_updated\":true}}{\"system\":{\"name_text_updated\":true}}");

            const string controllerIp = "192.168.1.181";
            IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Parse(controllerIp), 41100);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            var bytes = message.AsBytes();
            s.SendTo(bytes, bytes.Length, SocketFlags.None, RemoteEndPoint);
        }

        public void SendConnectUdp()
        {
            var data = "{\"location\":\"http://!!IP!!:!!DlnaHostPort!!/MediaRenderer/desc.xml\",\"ack\":\"http://!!IP!!:!!DlnaHostPort!!/MusicCastNetwork/InitialJoinComplete\"}".Replace("!!DlnaHostPort!!", MusicCastHost.DlnaHostPort.ToString());

            data = data
                     .Replace("!!IP!!", _environmentService.IpAddress)
                     .Replace("!!DlnaHostPort!!", MusicCastHost.DlnaHostPort.ToString());

            var message = new MulticastRequest(data);

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(_environmentService.SubnetBroadcastIp), 51100);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var bytes = message.AsBytes();
            s.SendTo(bytes, bytes.Length, SocketFlags.None, remoteEndPoint);
        }
    }
}
