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
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;

using Swimbait.Server.Services;
using Swimbait.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace Swimbait.Server
{
    public class Startup
    {
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

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var o = new StatusCodePagesOptions();
            o.HandleAsync = async statusCodeContext => {
                var request = statusCodeContext.HttpContext.Request;
                var context = statusCodeContext.HttpContext;
                var log = loggerFactory.CreateLogger<Startup>();
                log.LogWarning(request.Path);
                var uri = request.GetUri();

                var body = "<not decoded>";
                if (!uri.ToString().Contains("secure") && request.Method.ToLower() == "post") {
                    body = request.Form.Keys.First();
                }

                var isFavIcon = uri.AbsolutePath.EndsWith("favicon.ico");

                if (!isFavIcon) {
                    if (context.Response.HasStarted) {
                        log.LogCritical("Too late");
                    } else {
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
            app.UseStaticFiles();
            app.UseMvc();

        }
    }
}
