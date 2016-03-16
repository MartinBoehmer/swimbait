using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Server.Kestrel;
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
            builder.AddCommandLine(new[] { $"server.urls=http://192.168.1.3:80;http://192.168.1.3:{MusicCastHost.DlnaHostPort};http://192.168.1.3:51100" });
            var config = builder.Build();

            var webHost1 = new WebHostBuilder(config)
                .UseServer("Microsoft.AspNet.Server.Kestrel")
                .Build()
                .Start();



            //using ()
            {
                Console.WriteLine("Started the server..");
                Console.WriteLine("Press any key to stop the server");
            }

            //builder = new ConfigurationBuilder();
            //builder.AddCommandLine(new[] { "server.urls=http://localhost:5004" });
            //config = builder.Build();

            //var webHost2 = new WebHostBuilder(config)
            //    .UseServer("Microsoft.AspNet.Server.Kestrel")
            //    .Build()
            //    .Start();

            _multicastServer.Start();

            keyHandler.KeyEvent += KeyHandler_KeyEvent;

            keyHandler.WaitForExit();

            webHost1.Dispose();
            //webHost2.Dispose();
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
                    _multicastServer.JoinGroup();
                    break;
                case ConsoleKey.C:
                    _multicastService.SendPossiblyConnectUdp();
                    break;
                case ConsoleKey.V:
                    _multicastService.SendPossiblyConnectUdp2();
                    break;
            }

        }
    }
}