using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars5e.Starships.Parser.Processors;
using StarWars5e.Storage;

namespace StarWars5e.Starships.Parser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dependencies = Startup();
            var modificationProcessor = new StarshipModificationProcessor();
            var starshipSizeProcessor = new StarshipSizeProcessor();
            var starshipEquipmentProcessor = new StarshipEquipmentProcessor();
            var starshipVentureProcessor = new StarshipVentureProcessor();
            var starshipChapterRulesProcessor = new StarshipChapterRulesProcessor();
            var starshipDeploymentProcessor = new StarshipDeploymentProcessor();

            //var modifications = modificationProcessor.Process("modifications").Result;
            //dependencies.GetService<IModificationRepository>().Insert(modifications, "core").Wait();

            //var baseSizes = baseSizeProcessor.Process("starshipSizes").Result;

            //var equipment = starshipEquipmentProcessor.Process("equipment").Result;
            //var ventures = starshipVentureProcessor.Process("sotg").Result;

            var rules = starshipChapterRulesProcessor.Process("sotg").Result;
            //var deployments = starshipDeploymentProcessor.Process("sotg").Result;
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
