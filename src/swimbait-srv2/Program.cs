using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Server.Kestrel;

namespace ServersDemo
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;

        public Program(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<int> Main(string[] args)
        {
            //Add command line configuration source to read command line parameters.
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);
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

            builder = new ConfigurationBuilder();
            builder.AddCommandLine(new[] { "server.urls=http://localhost:5004" });
            config = builder.Build();

            var webHost2 = new WebHostBuilder(config)
                .UseServer("Microsoft.AspNet.Server.Kestrel")
                .Build()
                .Start();

            Console.ReadLine();

            webHost1.Dispose();
            webHost2.Dispose();

            return Task.FromResult(0);
        }
    }
}