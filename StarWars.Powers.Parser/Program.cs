using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Storage;
using StarWars.Storage.Repositories;
using StarWars5e.Factories;

namespace StarWars.Powers.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var di = Startup();
            Console.WriteLine("Hello World!");
            var repo = di.GetService<IPowerRepository>();
            var parser = new PowerParser(repo);
            var forcePowers = (parser.Process("force-powers", "force")).Result;
            var forceModels = forcePowers.Select(PowerFactory.ConvertToApiModel).ToList();
            repo.InsertPowers(forceModels).Wait();
            var techPowers = parser.Process("tech-powers", "tech").Result;
            var techModels = techPowers.Select(PowerFactory.ConvertToApiModel).ToList();
            repo.InsertPowers(techModels).Wait();
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
