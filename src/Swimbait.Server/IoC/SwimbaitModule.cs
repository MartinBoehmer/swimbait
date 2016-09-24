using System;
using System.Collections.Generic;
using Autofac;
using Swimbait.Server.Services;
using Swimbait.Common.Services;

namespace Swimbait.Server
{
    public class SwimbaitModule : BaseModule
    {
        protected override void Load(ContainerBuilder builder)
        {

            var services = new List<Type>();
            //services.Add(typeof(EnvironmentService));
            //services.Add(typeof(MusicCastHost));

            RegisterTypes(builder, "Services", services);
            
            var environmentService = new EnvironmentService();
            var musicCastHost = new MusicCastHost(environmentService);

            builder.RegisterInstance<EnvironmentService>(environmentService)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterInstance<MusicCastHost>(musicCastHost)
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}
