using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Storage;
using StarWars.Storage.Repositories;
using StarWars5e.Factories;

namespace StarWars.Species.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var di = Startup();
            var repo = di.GetService<ISpeciesRepository>();
            var parser = new SpeciesParser(repo);
            var species = (parser.Process("core", false, true).Result).ToList();
            var speciesModels = species.Select(SpeciesFactory.ConvertFromViewModel).ToList();
            var expanded = (parser.Process("expanded", false, true).Result).ToList();
            var expandedModels = expanded.Select(SpeciesFactory.ConvertFromViewModel).ToList();
            repo.InsertSpecies(speciesModels, "core").Wait();
            repo.InsertSpecies(expandedModels, "expanded").Wait();
            ;
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
