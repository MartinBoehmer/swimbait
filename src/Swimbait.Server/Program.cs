using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Swimbait.Server.Multicast;
using Swimbait.Server.Services;
using Swimbait.Common.Services;
using System.IO;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Swimbait.Server
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;
        private static MulticastServer _multicastServer;
        private static MulticastService _multicastService;

        public Program(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static void Main(string[] args)
        {
            var keyHandler = new KeyHandler();

            // todo: IOC
            var environmentService = new EnvironmentService();
            _multicastServer = new MulticastServer(environmentService);
            _multicastService = new MulticastService(environmentService);
            var _musicCastHost = new MusicCastHost(environmentService);
            
            //Add command line configuration source to read command line parameters.
            var builder = new ConfigurationBuilder();
            var portsToListen = new []{80, EnvironmentService.SwimbaitDlnaPort, 51100};

            var urisToListen = portsToListen
                                .ToList()
                                .Select(p => $"http://{environmentService.IpAddress}:{p}");

            var uriToListenString = string.Join(";", urisToListen);
        
            var config = builder
                .AddCommandLine(new[] { $"server.urls={uriToListenString}" })
                .AddEnvironmentVariables()
                .Build();

            /*
             * Reboot after setting this to be the IP of a real Yamaha MusicCast device on your network
             * Powershell below:
             [Environment]::SetEnvironmentVariable("Swimbait.RelayHost", "192.168.1.213", "Machine") 
             */
            _musicCastHost.RelayHost = IPAddress.Parse(config["Swimbait.RelayHost"]);

            // Dirty DI
            Startup._environmentService = environmentService;
            Startup._musicCastHost = _musicCastHost;

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
            
            Console.WriteLine($"Starting the server. Listening on {uriToListenString}. Udp broadcastinto to {environmentService.SubnetBroadcastIp}");
            host.Start();

            Console.WriteLine("Press 'Q' to stop the server");
            Console.WriteLine("Press 'M' to send SSDP Multicast discovery");
            Console.WriteLine("Press 'C' when ready to connect to the MusicCast app");

            _multicastServer.Start();

            keyHandler.KeyEvent += KeyHandler_KeyEvent;

            keyHandler.WaitForExit();

            host.Dispose();

            _multicastServer.Dispose();

        }

        private static void KeyHandler_KeyEvent(object sender, ConsoleKeyEventArgs e)
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