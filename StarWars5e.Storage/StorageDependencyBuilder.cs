using Autofac;
using StarWars.Storage.Clients;
using StarWars.Storage.Repositories;

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
            builder.RegisterType<PowerRepository>()
                .As<IPowerRepository>();

            builder.RegisterType<SpeciesRepository>()
                .As<ISpeciesRepository>();

            builder.RegisterType<BackgroundRepository>()
                .As<IBackgroundRepository>();
            builder.RegisterType<EquipmentRepository>()
                .As<IEquipmentRepository>();

            builder.RegisterType<ModificationRepository>()
                .As<IModificationRepository>();
            return builder;
        }
    }
}