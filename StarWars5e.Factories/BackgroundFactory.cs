using System;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Factories
{
    public static class BackgroundFactory
    {
        public static Background ConvertFromViewModel(BackgroundViewModel input)
        {
            var json = JsonConvert.SerializeObject(input);
            var model = JsonConvert.DeserializeObject<Background>(json);

            model.PartitionKey = "core";
            model.RowKey = Guid.NewGuid().ToString();
            return model;
        }

        public static BackgroundViewModel ConvertFromDataModel(Background input)
        {
            var vm = JsonConvert.DeserializeObject<BackgroundViewModel>(JsonConvert.SerializeObject(input));
            return vm;
        }
    }
}