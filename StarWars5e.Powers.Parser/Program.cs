using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Storage;
using StarWars5e.Storage.Repositories;

namespace StarWars5e.Powers.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var di = Startup();
            Console.WriteLine("Hello World!");
            var repo = di.GetService<IPowerRepository>();
            var powers = repo.GetAllPowers().Result;
            var txt = File.ReadAllText(@"C:\side\tags.json");
            var tags= JsonConvert.DeserializeObject<List<TagPowers>>(txt);
            var updated = UpdateTags(powers, tags);
            repo.InsertPowers(updated).Wait();
            //            var parser = new PowerParser(repo);
            //            var forcePowers = (parser.Process("force-powers", "force")).Result;
            //            var forceModels = forcePowers.Select(PowerFactory.ConvertToApiModel).ToList();
            //            repo.InsertPowers(forceModels).Wait();
            //            var techPowers = parser.Process("tech-powers", "tech").Result;
            //            var techModels = techPowers.Select(PowerFactory.ConvertToApiModel).ToList();
            //            repo.InsertPowers(techModels).Wait();
            ;
        }

        static List<Power> UpdateTags(List<Power> powers, List<TagPowers> tagList)
        {
            foreach (var tag in tagList)
            {
                var name = tag.Title;
                foreach (var item in tag.Items)
                {
                    var lower = item.ToLower();
                    var ix = powers.FindIndex(p => p.Name.ToLower() == lower);
                    if (ix < 0)
                    {
                        Console.WriteLine($"We couldn't find the index for {lower}");
                        ;
                    }
                    else
                    {
                        if (powers[ix].Tags == null)
                        {
                            powers[ix].Tags = new List<string>();
                        }

                        Console.WriteLine($"Added {name} to {powers[ix].Name}");
                        powers[ix].Tags.Add(name);
                    }
                }
            }

            return powers;
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

    public class TagPowers
    {
        /// <summary>
        ///  the name of the tag to apply
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///  The powers that have the tag
        /// </summary>
        [JsonProperty("items")]
        public List<string> Items { get; set; }

    }
}
