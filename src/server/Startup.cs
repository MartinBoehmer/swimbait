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
using Microsoft.AspNet.Diagnostics;
using AuthenticationSchemes = Microsoft.Net.Http.Server.AuthenticationSchemes;


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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            var webListenerInfo = app.ServerFeatures.Get<WebListener>();
            if (webListenerInfo != null)
            {
                webListenerInfo.AuthenticationManager.AuthenticationSchemes =
                    AuthenticationSchemes.AllowAnonymous;
            }

            //var serverAddress = app.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault();
            

            var loggingConfiguration = new ConfigurationBuilder()
                .AddJsonFile("logging.json")
                .Build();
            loggerFactory.AddConsole(loggingConfiguration);
            //loggerFactory.AddDebug();
            loggingConfiguration.ReloadOnChanged("logging.json");

            //app.UseIISPlatformHandler();

            //app.UseApplicationInsightsRequestTelemetry();

            //app.UseApplicationInsightsExceptionTelemetry();

            // app.UseStaticFiles();

            //app.UseStatusCodePages(context => context.HttpContext.Response.SendAsync("Handler, status code: " + context.HttpContext.Response.StatusCode, "text/plain"));

            //var o = new StatusCodePagesOptions();
            //o.
            app.UseStatusCodePagesWithReExecute("/Error/Status/{0}");
            app.UseMvc();
            
        }


        // Entry point for the application.
       // public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
