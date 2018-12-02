using Autofac;
using StarWars.Storage.Clients;

namespace StarWars.Storage
{
    public class StorageDependencyBuilder
    {
        public static ContainerBuilder RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<TableStorageConnection>()
                .As<ITableStorageConnection>()
                .SingleInstance();

            builder.RegisterType<MonsterRepository>()
                .As<IMonsterRepository>();

            return builder;
        }
    }
}