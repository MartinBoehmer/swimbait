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

            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables();
            var config = builder.Build();

            /*
            * Reboot after setting this to be the IP of a real Yamaha MusicCast device on your network
            * Powershell below:
            [Environment]::SetEnvironmentVariable("Swimbait.RelayHost", "192.168.1.213", "Machine") 
            */
            var musicCastSpeaker = config["Swimbait.RelayHost"];


            var speakerUrl = $"http://{musicCastSpeaker}";
            _musicCastClient = new MusicCastClient(speakerUrl);

            Console.WriteLine($"Started the client. Connecting to {speakerUrl}...");
            Console.WriteLine("Press 'P' for power toggle");
            Console.WriteLine("Press 'Q' to quit");


            keyHandler.KeyEvent += KeyHandler_KeyEvent;

            keyHandler.WaitForExit();

        }

        private static async void KeyHandler_KeyEvent(object sender, ConsoleKeyEventArgs e)
        {
            switch (e.KeyInfo.Key)
            {
                case ConsoleKey.P:
                    var status = await _musicCastClient.GetStatusAsync();
                    Console.WriteLine($"Status is {status}");
                    switch (status.power)
                    {
                        case "on":
                            await _musicCastClient.SetPowerAsync(false);
                            break;
                        default:
                            await _musicCastClient.SetPowerAsync(true);
                            break;
                    }
                    status = await _musicCastClient.GetStatusAsync();
                    Console.WriteLine($"Status is now {status.power}");
                    break;
            }
        }
    }
}
