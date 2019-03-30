using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Species;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers
{
    public class SpeciesProcessor : BaseProcessor<Species>
    {
        public override Task<List<Species>> FindBlocks(List<string> lines)
        {
            var species = new List<Species>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("> ## ") && !lines[i].StartsWith(">## ")) continue;
                
                var speciesEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("> ## ") || f.StartsWith(">## "));
                var speciesLines = lines.Skip(i).ToList();
                if (speciesEndIndex != -1)
                {
                    speciesLines = lines.Skip(i).Take(speciesEndIndex - i).CleanListOfStrings().ToList();
                }

                species.Add(ParseSpecies(speciesLines));
            }

            return Task.FromResult(species);
        }

        private static Species ParseSpecies(List<string> speciesLines)
        {
            var name = speciesLines.Find(f => f.StartsWith("> ## ") || f.StartsWith(">## ")).Split("## ")[1].Trim().RemoveMarkdownCharacters();
            try
            {
                var species = new Species
                {
                    ContentTypeEnum = ContentType.ExpandedContent,
                    Name = name,
                    RowKey = name,
                    PartitionKey = ContentType.ExpandedContent.ToString()
                };

                var descriptionStart = speciesLines.FindIndex(f => f.StartsWith("### "));
                var traitsStart = speciesLines.FindIndex(f => f.StartsWith("### ") && f.Contains("Traits"));
                species.FlavorText = string.Join("\r\n",
                    speciesLines.Skip(descriptionStart).Take(traitsStart - descriptionStart).ToList());

                var traitLines = speciesLines.Skip(traitsStart).ToList().Where(t => t.StartsWith("***"));

                species.ColorScheme = speciesLines.Find(f => f.Contains("***Color Scheme***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.SkinColorOptions = speciesLines.Find(f => f.Contains("***Skin Color***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.HairColorOptions = speciesLines.Find(f => f.Contains("***Hair Color***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.EyeColorOptions = speciesLines.Find(f => f.Contains("***Eye Color***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.Distinctions = speciesLines.Find(f => f.Contains("***Distinctions***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.HeightAverage = speciesLines.Find(f => f.Contains("***Height***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.HeightRollMod = speciesLines.Find(f => f.Contains("***Height***"))?.Split('|')
                    .Skip(4).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.WeightAverage = speciesLines.Find(f => f.Contains("***Weight***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.WeightRollMod = speciesLines.Find(f => f.Contains("***Weight***"))?.Split('|')
                    .Skip(4).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.Homeworld = speciesLines.Find(f => f.Contains("***Homeworld***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();              
                species.Manufacturer = speciesLines.Find(f => f.Contains("***Manufacturer***"))?.Split('|')
                    .Skip(2).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();

                species.Language = speciesLines.Find(f => f.Contains("***Language***"))?.Split('|')
                    .Skip(3).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                if (species.Language == null)
                {
                    species.Language = speciesLines.Find(f => f.Contains("***Primary Language***"))?.Split('|')
                        .Skip(2).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                }

                species.Traits = new List<Trait>();
                foreach (var traitLine in traitLines)
                {
                    var trait = new Trait();
                    trait.Name = traitLine.Split("***")[1].Trim();
                    trait.Description = traitLine.Split("***")[2].Trim().RemoveHtmlWhitespace();
                    species.Traits.Add(trait);
                }

                return species;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }
    }
}
