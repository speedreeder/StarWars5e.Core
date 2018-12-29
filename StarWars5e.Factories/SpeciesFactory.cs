using System;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Factories
{
    public static class SpeciesFactory
    {
        public static Species ConvertFromViewModel(SpeciesViewModel input)
        {
            var model = JsonConvert.DeserializeObject<Species>(JsonConvert.SerializeObject(input));

            model.PartitionKey = "core";
            model.RowKey = Guid.NewGuid().ToString();
            return model;
        }

        public static SpeciesViewModel ConvertFromDataModel(Species input)
        {
            var vm = JsonConvert.DeserializeObject<SpeciesViewModel>(JsonConvert.SerializeObject(input));
            return vm;
        }
    }
}