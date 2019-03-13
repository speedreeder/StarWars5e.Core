using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Starships.Parser.Processors.Modifications;
using StarWars.Storage;

namespace StarWars.Starships.Parser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var modificationProcessor = new CoreModificationProcessor();
            modificationProcessor.Process("modifications").Wait();
        }

        public static AutofacServiceProvider Startup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);
            StorageDependencyBuilder.RegisterTypes(containerBuilder);

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}
