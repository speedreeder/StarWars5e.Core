using System.Collections.Generic;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Background.Parser.Processors
{
    public class CoreProcessor
    {
        private List<IBackgroundProcessor> processors;

        public CoreProcessor()
        {
            this.BuildProcessorList();
        }
        public BackgroundViewModel ConvertMarkdownToBackground(List<string> content)
        {
            IBackgroundProcessor currentProcessor = null;
            List<string> currentSection = new List<string>();
            var vm = new BackgroundViewModel();


            foreach (var line in content)
            {
                if (this.ShouldIgnore(line))
                {
                    continue;
                }

                var proc = this.DetermineCurrentProcessor(line);

                if (currentProcessor == null)
                {
                    currentProcessor = proc;
                }

                if (proc != null && currentProcessor != null
                                 && proc.Section != currentProcessor.Section)
                {
                    // the processor changed. So process whatever existed and then reset
                    vm = currentProcessor.Process(currentSection, vm);

                    currentSection = new List<string>();
                    currentProcessor = proc;
                }

                currentSection.Add(line);
            }

            if (currentSection.Count > 0)
            {
                vm = currentProcessor.Process(currentSection, vm);
            }

            return vm;
        }

        private IBackgroundProcessor DetermineCurrentProcessor(string line)
        {
            foreach (var processor in this.processors)
            {
                if (processor.IsMatch(line))
                {
                    return processor;
                }
            }

            return null;
        }

        private void BuildProcessorList()
        {
            this.processors = new List<IBackgroundProcessor>
            {
                new NameProcessor(),
                new SkillProficiencyProcessor(),
                new ToolProficiencyProcessor(),
                new LanguageProcessor(),
                new FeatProcessor(),
                new FeatureProcessor(),
                new BondProcessor(),
                new IdealProcessor(),
                new SpecialityProcessor(),
                new EquipmentProcessor(),
                new FlawProcessor(),
                new PersonalityProcessor()
            };
        }

        private bool ShouldIgnore(string line)
        {
            return string.IsNullOrEmpty(line)
                || line == "|:---:|:----------|"
                || line == "|:---:|:----------:|"
                || line == "|:---:|:---------|"
                || line == "|:----:|:-------------|"
                || line == "|:--:|:--:|"
                || line == "|:--:|:--|"
                || line == "___";
        }
    }
}