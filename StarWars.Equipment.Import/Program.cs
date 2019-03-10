using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Storage;
using StarWars.Storage.Repositories;

namespace StarWars.Equipment.Import
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var di = Startup();
            var proc = new EquipmentProcessor(di.GetService<IEquipmentRepository>());
            //proc.Process("errata", false, true).Wait();
            proc.Process("armors", false, true).Wait();
            proc.Process("assorted", false, true).Wait();
            proc.Process("containers", false, true).Wait();
            proc.Process("mounts", false, true).Wait();
            proc.Process("tools", false, true).Wait();
            proc.Process("weapons", false, true).Wait();
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
