using System;
using System.IO;
using MusicCast;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Swimbait.Common;

namespace MusicCast.ConsoleApp
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;
        private static MusicCastClient _musicCastClient;
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
            var speakerUrl = "http://192.168.1.7";
            _musicCastClient = new MusicCastClient(speakerUrl);

            //var config = builder
            //    .AddCommandLine(new[] { $"server.urls={speakerUrl}" })
            //    .AddEnvironmentVariables(prefix: "ASPNETCORE_")
            //    .Build();

            //var host = new WebHostBuilder()
            //    .UseConfiguration(config)
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .Build();

            Console.WriteLine($"Started the client. Connecting to {speakerUrl}...");
            Console.WriteLine("Press 'C' to connect and turn on");
            Console.WriteLine("Press 'D' to turn off");
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
            case ConsoleKey.D: {
                    Console.WriteLine("SendDisconnectConnect");
                    var status = await _musicCastClient.GetStatusAsync();
                    Console.WriteLine($"Status  power is {status.power}");
                    if (status.power == "on") {
                        Console.WriteLine($"Turning off");
                        await _musicCastClient.SetPowerAsync(false);
                        status = await _musicCastClient.GetStatusAsync();
                        Console.WriteLine($"Status  power is now {status.power}");
                    }
                    break;
                }
            case ConsoleKey.C: {
                    Console.WriteLine("SendConnect");
                    var sucess = await _musicCastClient.ConnectAsync();
                    Console.WriteLine($"Connection  {(sucess ? "ok" : "fail")}");

                    var status = await _musicCastClient.GetStatusAsync();
                    Console.WriteLine($"Status  power is {status.power}");
                    if (status.power == "standby") {
                        Console.WriteLine($"Turning on");
                        await _musicCastClient.SetPowerAsync(true);
                        status = await _musicCastClient.GetStatusAsync();
                        Console.WriteLine($"Status  power is now {status.power}");
                    }
                    break;
                }

            case ConsoleKey.V:
                Console.WriteLine("SendNotSureWhatThisDoesUdp");
                break;
            }
        }
    }
}
