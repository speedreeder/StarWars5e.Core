using Autofac;
using StarWars5e.Storage.Clients;
using StarWars5e.Storage.Repositories;

namespace StarWars5e.Storage
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