using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Server;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNet.Extensions;
using Microsoft.AspNet.Diagnostics;
using AuthenticationSchemes = Microsoft.Net.Http.Server.AuthenticationSchemes;
using System.IO;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNet.Mvc;
using Swimbait.Server.Services;

namespace Swimbait.Server
{
    public class Startup
    {

        public Startup(IHostingEnvironment env,
            IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }

        public IConfiguration Configuration { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<MvcOptions>(options =>
            {
            });
            
            var serviceProvider = BuildIoC(services);

            return serviceProvider;
        }


        private IServiceProvider BuildIoC(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule(new SwimbaitModule());

            builder
                .Register(c => Configuration)
                .SingleInstance();

            builder.Populate(services);
            var container = builder.Build();

            // Resolve and return the service provider
            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            var webListenerInfo = app.ServerFeatures.Get<WebListener>();
            if (webListenerInfo != null)
            {
                webListenerInfo.AuthenticationManager.AuthenticationSchemes = AuthenticationSchemes.AllowAnonymous;
            }

            
            var loggingConfiguration = new ConfigurationBuilder()
                .AddJsonFile("logging.json")
                .Build();
            loggerFactory.AddConsole(loggingConfiguration);
            loggingConfiguration.ReloadOnChanged("logging.json");
            
            var o = new StatusCodePagesOptions();
            o.HandleAsync = async context =>
            {
                var log = loggerFactory.CreateLogger<Startup>();
                log.LogWarning(context.HttpContext.Request.Path);
                var uri = context.HttpContext.Request.GetUri();

                var isFavIcon = uri.AbsolutePath.EndsWith("favicon.ico");
                
                if (!isFavIcon)
                {
                    using (var httpClient = new HttpClient())
                    {

                        // remap the port since windows is using 49154
                        var relayPort = uri.Port == MusicCastHost.DlnaHostPort ? 49154 : uri.Port;

                        var relayUri = new Uri($"http://{MusicCastHost.RelayHost}:{relayPort}" + uri.PathAndQuery);

                        var result = await httpClient.GetStringAsync(relayUri);

                        var debugFolder = @"D:\Downloads\swimbait\replay";
                        Directory.CreateDirectory(debugFolder);
                        var debugFile = Path.Combine(debugFolder, uri.GetHashCode() + ".txt");

                        var sb = new StringBuilder();
                        sb.Append("Url=");
                        sb.AppendLine(uri.AbsolutePath);
                        sb.Append(result);

                        File.WriteAllText(debugFile, sb.ToString());

                        if (!context.HttpContext.Response.HasStarted)
                        {
                            log.LogInformation($"Man in the middle! {relayUri}");
                            context.HttpContext.Response.StatusCode = 200;
                            await context.HttpContext.Response.WriteAsync(result);
                        }
                    }
                }
            };

            app.UseStatusCodePages(o);
            
            app.UseMvc();

        }
    }
}
