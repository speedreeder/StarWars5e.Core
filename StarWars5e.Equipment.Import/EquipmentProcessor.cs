using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarWars.Storage.Repositories;
using StarWars5e.Factories;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Interfaces;
using StarWars5e.Models.ViewModels;

namespace StarWars.Equipment.Import
{
    public class EquipmentProcessor
    {
        private readonly IEquipmentRepository _repo;

        public EquipmentProcessor(IEquipmentRepository repo)
        {
            _repo = repo;
        }
        public async Task Process(string location, bool isRemote = false, bool isInternal = false)
        {
            var viewModels = new List<IEquipment>();

            if (isRemote)
            {
                viewModels = await this.ReadRemoteFile(location);
            }
            else if (isInternal)
            {
                viewModels = await this.ReadInternalFile(location);
            }

            ;

            foreach (var item in viewModels)
            {
                Console.WriteLine(item.Name);
                await this._repo.InsertItem(ConvertToEntity(item));
            }
        }

        private IEquipment ConvertToEntity(IEquipment input)
        {
            switch (input.Category)
            {
                
                case (int)ItemCategory.Weapon:
                    return EquipmentFactory.ConvertWeaponViewModel(input as WeaponViewModel);
                case (int)ItemCategory.Armor:
                    return EquipmentFactory.ConvertArmorViewModel(input as ArmorViewModel);
                case (int)ItemCategory.AdventurePack:
                    return EquipmentFactory.ConvertAdventurePackViewModel(input as AdventurePackViewModel);
                case (int)ItemCategory.Mount:
                    return EquipmentFactory.ConvertMountViewModel(input as MountViewModel);
                case (int)ItemCategory.Vehicle:
                    return EquipmentFactory.ConvertVehicleViewModel(input as VehicleViewModel);
                case (int)ItemCategory.Container:
                    return EquipmentFactory.ConvertContainerViewModel(input as ContainerViewModel);
                case (int)ItemCategory.Communications:
                case (int)ItemCategory.DataRecorder:
                case (int)ItemCategory.LifeSupport:
                case (int)ItemCategory.MedicalSupply:
                case (int)ItemCategory.WeaponOrArmorAccessory:
                case (int)ItemCategory.Tool:
                case (int)ItemCategory.Unsorted:
                case (int)ItemCategory.Ammunition:
                case (int)ItemCategory.Explosive:
                case (int)ItemCategory.TradeGood:
                case (int)ItemCategory.Utilities:
                case (int)ItemCategory.GamingSet:
                case (int)ItemCategory.MusicalInstrument:
                case (int)ItemCategory.Droid:
                default:
                    return EquipmentFactory.ConvertItemViewModel(input as ItemViewModel);
            }
        }

        private async Task<List<IEquipment>> ReadInternalFile(string location)
        {
            try
            {
                var items = new List<IEquipment>();
                var ass = Assembly.GetEntryAssembly();
                var res = ass.GetManifestResourceNames();
                using (Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream($"StarWars.Equipment.Import.Sources.{location}.json"))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 128))
                    {
                        
                        using (JsonReader jsonReader = new JsonTextReader(reader))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            items = serializer.Deserialize<List<IEquipment>>(jsonReader);
                        }
                    }
                }

                return items;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Process a remote file and turn it into a list of strings to be read
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private async Task<List<IEquipment>> ReadRemoteFile(string location)
        {
            throw new NotImplementedException();
        }
    }
}
