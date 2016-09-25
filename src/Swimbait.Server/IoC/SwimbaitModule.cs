using System;
using System.Collections.Generic;
using System.Net;
using Autofac;
using Swimbait.Server.Services;
using Swimbait.Common.Services;

namespace Swimbait.Server
{
    public class SwimbaitModule : BaseModule
    {
        /// <summary>
        /// temp hack - shouldn't be internal
        /// </summary>
        internal static EnvironmentService GetEnvironmentService()
        {
            var config = SwimbaitConfig.Get();
            var environmentService = new EnvironmentService(config);
            return environmentService;
        }

        protected override void Load(ContainerBuilder builder)
        {

            var services = new List<Type>();
            //services.Add(typeof(EnvironmentService));
            //services.Add(typeof(MusicCastHost));
            var swimbaitConfig = SwimbaitConfig.Get();

            RegisterTypes(builder, "Services", services);

            var environmentService = GetEnvironmentService();
            var musicCastHost = new MusicCastHost(environmentService);

            musicCastHost.RelayHost = IPAddress.Parse(swimbaitConfig["RelayHost"]);


            builder.RegisterInstance<EnvironmentService>(environmentService)
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterInstance<MusicCastHost>(musicCastHost)
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}
