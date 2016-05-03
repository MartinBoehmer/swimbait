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
using Swimbait.Common;

namespace Swimbait.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }

        public IConfiguration Configuration { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<MvcOptions>(options => {});
            
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
            o.HandleAsync = async statusCodeContext =>
            {
                var request = statusCodeContext.HttpContext.Request;
                var context = statusCodeContext.HttpContext;
                var log = loggerFactory.CreateLogger<Startup>();
                log.LogWarning(request.Path);
                var uri = request.GetUri();

                var body = "<not decoded>";
                if (!uri.ToString().Contains("secure") && request.Method.ToLower() == "post")
                {
                    body = request.Form.Keys.First();
                }

                var isFavIcon = uri.AbsolutePath.EndsWith("favicon.ico");

                if (!isFavIcon )
                {
                    if (context.Response.HasStarted)
                    {
                        log.LogCritical("Too late");
                    }
                    else
                    {
                        var manInTheMiddleResponse = UriService.GetManInTheMiddleResult(MusicCastHost.RelayHost, uri, ActivityLogMiddleware.MapPortToReal);
                        log.LogInformation($"Man in the middle! {manInTheMiddleResponse.RequestUri}");
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync(manInTheMiddleResponse.ResponseBody);

                        var logService = new LogService();
                        logService.LogToDisk(-1, manInTheMiddleResponse);
                    }
                }
            };

            app.UseStatusCodePages(o);

            app.UseHeadersMiddleware();

            app.UseActivityLogMiddleware();

            app.UseMvc();

        }
    }
}
