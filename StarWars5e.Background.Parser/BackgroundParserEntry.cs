using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StarWars5e.Storage;
using StarWars5e.Storage.Repositories;

namespace StarWars5e.Background.Parser
{
    class BackgroundParserEntry
    {
        static void Main(string[] args)
        {
            var di = Startup();
            var repo = di.GetService<IBackgroundRepository>();
            var parser = new BackgroundDocumentParser();
            var backgrounds = (parser.Process("expanded-content", false, true).Result).ToList();
//            var speciesModels = backgrounds.Select(BackgroundFactory.ConvertFromViewModel).ToList();
            repo.InsertBackground(backgrounds).Wait();
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
