using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace StarWars5e.Common
{
    public class DependencyContainerBuilder
    {
        public static void Bob()
        {
            var containerBuilder = new ContainerBuilder();
            var container = containerBuilder.Build();
            
        }
    }
}
