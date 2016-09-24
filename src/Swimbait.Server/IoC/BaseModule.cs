using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace Swimbait.Server
{
    public abstract class BaseModule : Module
    {
        protected void RegisterTypes(ContainerBuilder builder, string friendlyName, List<Type> types)
        {
            //Log.InfoFormat(
            //  "Registering {0} {2}: {1}"
            //  , types.Count
            //  , GetTypeNameCsv(types)
            //  , friendlyName);

            foreach (var repoType in types)
            {
                builder.RegisterType(repoType)
                        .AsImplementedInterfaces()
                        .AsSelf();
            }
        }

        //public string GetTypeNameCsv(IEnumerable<Type> types)
        //{
        //    //return <Type>.GetCsv(types, t => t.Name, ", ");
        //}
    }
}
