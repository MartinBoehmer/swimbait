using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Server;
using System;
using System.Linq;
using AuthenticationSchemes = Microsoft.Net.Http.Server.AuthenticationSchemes;
using System.IO;
using System.Net;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;

using Swimbait.Server.Services;
using Swimbait.Common;
using Swimbait.Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging.Console;

namespace Swimbait.Server
{
    public class Startup
    {
        /// <summary>
        /// this is a really dirty hack while i work out how to DI into StatusPAges
        /// </summary>
        internal static IEnvironmentService _environmentService;

        /// <summary>
        /// another dirty hack
        /// </summary>
        internal static MusicCastHost _musicCastHost;

        public Startup(IHostingEnvironment env)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }

        public IConfiguration Configuration { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<MvcOptions>(options => { });

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
                webListenerInfo.Settings.Authentication.Schemes = AuthenticationSchemes.None;

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            var log2 = loggerFactory.CreateLogger("test");
            log2.LogWarning("log test");


            var o = new StatusCodePagesOptions();
            o.HandleAsync = async statusCodeContext => {
                var request = statusCodeContext.HttpContext.Request;
                var context = statusCodeContext.HttpContext;
                var log = loggerFactory.CreateLogger<Startup>();
                log.LogWarning($"{request.Path} wasn't handled.");
                var uri = request.GetUri();

                var body = "<not decoded>";
                if (!uri.ToString().Contains("secure") && request.Method.ToLower() == "post") {
                    //body = request.Form.Keys.First();
                }

                var isFavIcon = uri.AbsolutePath.EndsWith("favicon.ico");
                
                //not the singleton instance var musicCastHost = (MusicCastHost) app.ApplicationServices.GetService(typeof (MusicCastHost));

                if (!isFavIcon) {
                    if (context.Response.HasStarted) {
                        log.LogCritical("Too late");
                    } else {
                        var manInTheMiddleResponse = UriService.GetManInTheMiddleResult(_musicCastHost.RelayHost, uri, ActivityLogMiddleware.MapPortToReal);
                        log.LogInformation($"Man in the middle! {manInTheMiddleResponse.RequestUri}");
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync(manInTheMiddleResponse.ResponseBody);

                        var logService = new LogService(_environmentService);
                        logService.LogToDisk(-1, manInTheMiddleResponse);
                    }
                }
            };

            app.UseStatusCodePages(o);

            app.UseHeadersMiddleware();

            app.UseActivityLogMiddleware();
            app.UseStaticFiles();
            app.UseMvc();

        }
    }
}
