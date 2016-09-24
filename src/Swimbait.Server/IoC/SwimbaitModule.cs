using System;
using System.Collections.Generic;
using Autofac;
using Swimbait.Server.Services;

namespace Swimbait.Server
{
    public class SwimbaitModule : BaseModule
    {
        protected override void Load(ContainerBuilder builder)
        {

            var services = new List<Type>();
            services.Add(typeof(EnvironmentService));

            RegisterTypes(builder, "Services", services);

            builder
                .Register(context => context.Resolve<MusicCastHost>())
                .SingleInstance();
        }
    }
}
