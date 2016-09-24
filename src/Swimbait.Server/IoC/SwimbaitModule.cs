using Autofac;
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
