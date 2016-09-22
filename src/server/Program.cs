using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Swimbait.Server.Multicast;
using Swimbait.Server.Services;
using System.IO;

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
            _multicastServer = new MulticastServer();
            _multicastService = new MulticastService();

            //Add command line configuration source to read command line parameters.
            var builder = new ConfigurationBuilder();
            var portsToListen = new []{80, MusicCastHost.DlnaHostPort, 51100};
            var urisToListen = portsToListen.ToList().Select(p => $"http://{MusicCastHost.ThisIp}:{p}");
            var uriToListenString = string.Join(";", urisToListen);
        
            var config = builder
                .AddCommandLine(new[] { $"server.urls={uriToListenString}" })
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            Console.WriteLine($"Started the server. Listing on {uriToListenString}");
            Console.WriteLine("Press any key to stop the server");

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