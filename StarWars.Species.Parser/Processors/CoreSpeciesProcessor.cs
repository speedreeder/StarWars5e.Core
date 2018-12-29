using System;
using System.Collections.Generic;
using StarWars5e.Models.ViewModels;

namespace StarWars.Species.Parser.Processors
{
    /// <summary>
    /// This processor takes a block that represents a species and builds an actual model from the text 
    /// </summary>
    public class CoreSpeciesProcessor
    {
        public CoreSpeciesProcessor()
        {
            
        }

        public SpeciesViewModel CreateSpecies(List<string> block)
        {
            var vm = new SpeciesViewModel();
            var currentSection = new List<string>();
            Tuple<ISpeciesProcessor, bool> currentProcessor = null;
            foreach (var line in block)
            {
                if (GuardChecks(line, currentSection))
                {
                    continue;
                }

                var shouldProcessContent = false;
                var proc = this.FindMatchingProcessor(line);

                if (currentProcessor == null && proc != null) // we found something, but we don't have anything set
                {
                    currentProcessor = proc;
                }

                if (proc != null 
                    && currentProcessor != null
                    && currentProcessor.Item1.SectionName != proc.Item1.SectionName) // they ar eboth populated but the processors don't match
                {
                    shouldProcessContent = true;
                }

                if (currentProcessor != null && currentProcessor.Item1.SectionName == SpeciesSection.CommonNames) // check weird special cases
                {
                    if (line.StartsWith("###"))
                    {
                        shouldProcessContent = true;
                        proc = null;
                    }
                }


                if (shouldProcessContent)
                {
                    // process what was there...
                    vm = currentProcessor.Item1.ProcessSection(currentSection, vm);

                    // set the values of the watchers to whatever is new!
                    currentSection = new List<string>();
                    currentProcessor = proc;
                    if (currentProcessor != null)
                    {
                        currentSection.Add(line);
                    }
                }
                else
                {
                    // we aren't ready to process yet, so let's roll on!
                    currentSection.Add(line);
                }
            }

            if (currentProcessor != null && currentSection.Count > 0)
            {
                // there is something left and we should use the current processor to crush it!
                currentProcessor.Item1.ProcessSection(currentSection, vm);
            }

            return vm;
        }

        /// <summary>
        /// Check if there is any reason to not continue
        /// </summary>
        /// <param name="line"></param>
        /// <param name="currentSection"></param>
        /// <returns></returns>
        private bool GuardChecks(string line, List<string> currentSection)
        {
            return line == "___"
                   || string.IsNullOrEmpty(line)
                   || line == "> ___"
                   || line == "> |:--|:--|:--:|:--:|"
                   || line == "> |:--|:--|:--|"
                   || line == "> ||||"
                   || line == "> |||||";
        }

        private Tuple<ISpeciesProcessor, bool> FindMatchingProcessor(string line)
        {
            foreach (var option in this.processors)
            {
                if (option.Item1.IsMatch(line))
                {
                    return option;
                }
            }

            return null;
        }

        private List<Tuple<ISpeciesProcessor, bool>> processors = new List<Tuple<ISpeciesProcessor, bool>>
        {
            new Tuple<ISpeciesProcessor, bool>(new NameProcessor(), false),
            new Tuple<ISpeciesProcessor, bool>(new VisualProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new DroidNoteProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new UtilityProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new VisualProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new DroidAppearanceProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new PhysicalProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new SocioCulturalProcess(), true),
            new Tuple<ISpeciesProcessor, bool>(new BiologyProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new SocietyProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new NamesProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new AbilityIncreaseProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new AgeProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new AlignmentProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new SizeProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new SpeedProcessor(), true),
            new Tuple<ISpeciesProcessor, bool>(new AttributesProcessor(), true)
        };
    }
}