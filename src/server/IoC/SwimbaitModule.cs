using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Swimbait.Server.Services;

namespace Swimbait.Server
{
    public class SwimbaitModule : BaseModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(context => new MusicCastHost())
                .SingleInstance();
        }
    }
}
