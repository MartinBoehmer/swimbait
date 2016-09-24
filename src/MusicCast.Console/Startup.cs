using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Client
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

        //    builder.RegisterModule(new SwimbaitModule());

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
          
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

         
            app.UseStaticFiles();
            app.UseMvc();

        }
    }
}