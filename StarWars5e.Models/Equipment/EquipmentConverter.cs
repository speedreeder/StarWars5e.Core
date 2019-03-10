using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Interfaces;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Models.Equipment
{
    public class EquipmentConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var item = default(IEquipment);
            switch (jsonObject["category"].Value<int>())
            {
                case (int)ItemCategory.Weapon:
                    item = new WeaponViewModel();
                    break;
                case (int)ItemCategory.AdventurePack:
                    item = new AdventurePackViewModel();
                    break;
                case (int)ItemCategory.Armor:
                    item = new ArmorViewModel();
                    break;
                case (int)ItemCategory.Container:
                    item = new ContainerViewModel();
                    break;
                case (int)ItemCategory.Mount:
                    item = new MountViewModel();
                    break;
                case (int)ItemCategory.Vehicle:
                    item = new VehicleViewModel();
                    break;
                default:
                    item = new ItemViewModel();
                    break;
            }
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
