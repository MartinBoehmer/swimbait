using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Swimbait.Common.Services
{
    public class SwimbaitConfig
    {
        public static IConfigurationSection Get()
        {
            var builder = new ConfigurationBuilder()
                                .AddEnvironmentVariables();

            var config = builder.Build();

            var configSection = config.GetSection("Swimbait");
            return configSection;
        }
    }
}
