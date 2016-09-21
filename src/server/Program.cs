using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Swimbait.Server.Multicast;
using Swimbait.Server.Services;

namespace Swimbait.Server
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;
        private MulticastServer _multicastServer;
        private MulticastService _multicastService;

        public Program(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _multicastService = new MulticastService();
        }

        public Task<int> Main(string[] args)
        {
            var keyHandler = new KeyHandler();
            _multicastServer = new MulticastServer();

            //Add command line configuration source to read command line parameters.
            var builder = new ConfigurationBuilder();
            var portsToListen = new []{80, MusicCastHost.DlnaHostPort, 51100};
            var urisToListen = portsToListen.ToList().Select(p => $"http://{MusicCastHost.ThisIp}:{p}");
            var uriToListenString = string.Join(";", urisToListen);
            builder.AddCommandLine(new[] { $"server.urls={uriToListenString}" });
            var config = builder.Build();

            var webHost1 = new WebHostBuilder().Build();
           
            webHost1.Start();
                //.UseServer("Microsoft.AspNet.Server.Kestrel")
                //.Build()
                //.Start();

            Console.WriteLine($"Started the server. Listing on {uriToListenString}");
            Console.WriteLine("Press any key to stop the server");

            _multicastServer.Start();

            keyHandler.KeyEvent += KeyHandler_KeyEvent;

            keyHandler.WaitForExit();

            webHost1.Dispose();

            _multicastServer.Dispose();

            return Task.FromResult(0);
        }

        private void KeyHandler_KeyEvent(object sender, ConsoleKeyEventArgs e)
        {
            switch (e.KeyInfo.Key)
            {
                case ConsoleKey.M:
                    _multicastServer.SsdpDiscover();
                    break;
                case ConsoleKey.J:
                    Console.WriteLine("JoinGroup");
                    _multicastServer.JoinGroup();
                    break;
                case ConsoleKey.C:
                    Console.WriteLine("SendConnectUdp");
                    _multicastService.SendConnectUdp();
                    break;
                case ConsoleKey.V:
                    Console.WriteLine("SendNotSureWhatThisDoesUdp");
                    _multicastService.SendNotSureWhatThisDoesUdp();
                    break;
            }
        }
    }
}