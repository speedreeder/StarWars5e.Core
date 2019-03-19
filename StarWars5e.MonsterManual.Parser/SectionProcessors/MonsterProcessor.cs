using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    public class MonsterProcessor
    {
        private List<string> currentSectionContent = new List<string>();
        private readonly string content;
        private bool featureTriggered = false;
        private Monster monster;

        public MonsterProcessor(string input)
        {
            this.content = input;
            this.monster = new Monster();
        }

        public Monster ProcessLineList(List<string> lines)
        {
            this.monster = new Monster();
            monster.OwnerName = "Jawa";
            var sectionType = MonsterSections.Unknown;
            foreach (var line in lines)
            {
                switch (sectionType)
                {
                    case MonsterSections.Features:
                        var reallyFeature = SectionMatchers.FeatureProc.DoubleCheck(line);
                        if (reallyFeature || this.currentSectionContent.Count > 0)
                        {
                            sectionType = this.DoFeatureThings(line, MonsterSections.Features);
                        }
                        else
                        {
                            sectionType = this.DetermineSection(line); // this is super new: 11/29 at 10pm
                        }
                        break;
                    case MonsterSections.Actions:
                        sectionType = this.DoFeatureThings(line, MonsterSections.Actions);
                        break;
                    case MonsterSections.Reactions:
                        sectionType = this.DoFeatureThings(line, MonsterSections.Reactions);
                        break;
                    case MonsterSections.LegendaryActions:
                        sectionType = this.DoFeatureThings(line, MonsterSections.LegendaryActions);
                        break;
                    default:
                        sectionType = this.DetermineSection(line);
                        this.featureTriggered = sectionType == MonsterSections.Features;
                        break;
                }
            }
            return this.monster;
        }

        public Monster Process()
        {
            try
            {
                var lines = Regex.Split(this.content, "\r\n|\r|\n").ToList();
                return this.ProcessLineList(lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private MonsterSections DoFeatureThings(string line, MonsterSections section)
        {
            if (line != ">" && line != "> ___")
            {
                if (!line.Contains("###") && line != "")
                {
                    this.currentSectionContent.Add(line);
                }
                else
                {
                    switch (section)
                    {
                        case MonsterSections.Features:
                            this.monster = SectionMatchers.FeatureProc.Process(this.monster, this.currentSectionContent);
                            break;
                        case MonsterSections.Actions:
                            this.monster = SectionMatchers.ActionProc.Process(this.monster, this.currentSectionContent);
                            break;
                        case MonsterSections.Reactions:
                            this.monster = SectionMatchers.ReactionProc.Process(this.monster, this.currentSectionContent);
                            break;
                        case MonsterSections.LegendaryActions:
                            this.monster = SectionMatchers.LegendaryActionProc.Process(this.monster, this.currentSectionContent);
                            break;
                    }
                    this.currentSectionContent = new List<string>();
                    return this.DetermineSection(line);
                }
            }

            return section;
        }

        private MonsterSections DetermineSection(string input)
        {
            var foundType = MonsterSections.Unknown;
            foreach (var sectionProcessor in SectionMatchers.Matchers)
            {
                foundType = sectionProcessor.IsMatch(input);
                if (foundType != MonsterSections.Unknown)
                {
                    this.monster = sectionProcessor.Process(this.monster, input);
                    break;
                }

            }


            return foundType;
        }
    }
}