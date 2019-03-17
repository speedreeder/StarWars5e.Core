using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Storage;

namespace StarWars.MonsterManual.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var di = Startup();
            Console.WriteLine("Hello World!");
            var repo = di.GetService<IMonsterRepository>();
            var parser = new ManualParser(repo);
            parser.ReadFile(@"C:\side\MM.txt").Wait();
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
