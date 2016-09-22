using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Swimbait.Common;
using System.IO;

namespace Client
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;
        private static YamahaService _yamahaService;
        //private static MulticastService _multicastService;

        public Program(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static void Main(string[] args)
        {
            var keyHandler = new KeyHandler();
            //Add command line configuration source to read command line parameters.
            var builder = new ConfigurationBuilder();
            var uriToListenString = "http://192.168.1.7";
            _yamahaService = new YamahaService(uriToListenString);

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

            Console.WriteLine($"Started the client. Connecting to {uriToListenString}...");
            Console.WriteLine("Press 'Q' to quit");


            keyHandler.KeyEvent += KeyHandler_KeyEvent;

            keyHandler.WaitForExit();

            //host.Dispose();

            //_multicastServer.Dispose();

        }

        private static async void KeyHandler_KeyEvent(object sender, ConsoleKeyEventArgs e)
        {
            switch (e.KeyInfo.Key) {
            case ConsoleKey.M:
                break;
            case ConsoleKey.J:
                Console.WriteLine("JoinGroup");
                break;
            case ConsoleKey.C: {
                    Console.WriteLine("SendConnect");
                    var sucess = await _yamahaService.ConnectAsync();
                    Console.WriteLine($"Connection  {(sucess ? "ok" : "faile")}");
                    break;
                }

            case ConsoleKey.V:
                Console.WriteLine("SendNotSureWhatThisDoesUdp");
                break;
            }
        }
    }
}
