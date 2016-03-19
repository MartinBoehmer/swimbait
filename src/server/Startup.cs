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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            var webListenerInfo = app.ServerFeatures.Get<WebListener>();
            if (webListenerInfo != null)
            {
                webListenerInfo.AuthenticationManager.AuthenticationSchemes = AuthenticationSchemes.AllowAnonymous;
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

            var o = new StatusCodePagesOptions();
            o.HandleAsync = async context =>
            {
                var log = loggerFactory.CreateLogger<Startup>();
                log.LogWarning(context.HttpContext.Request.Path);
                var uri = context.HttpContext.Request.GetUri();

                var isFavIcon = uri.AbsolutePath.EndsWith("favicon.ico");
                
                //var relayRequest = HttpWebRequest.Create(context.HttpContext.Request.GetUri());
                //var relayResponse = relayRequest.GetResponse();
                //relayResponse.

                if (!isFavIcon)
                {
                    using (var httpClient = new HttpClient())
                    {


                        var relayUri = new Uri($"http://{MusicCastHost.RelayHost}" + uri.PathAndQuery);

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
                            await context.HttpContext.Response.WriteAsync(result);
                        }
                    }
                }
            };

            app.UseStatusCodePages(o);
            
            //app.UseStatusCodePages("/Error/Status/{0}");
            app.UseMvc();
            
        }


        // Entry point for the application.
       // public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
