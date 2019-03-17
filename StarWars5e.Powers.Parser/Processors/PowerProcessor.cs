using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Powers.Parser.Processors
{
    /// <summary>
    /// Class that takes an individual power block and converts it from markdown into usable data
    /// </summary>
    public class PowerProcessor
    {
        private List<Tuple<Regex, IPowerProcessor>> SectionGuide = new List<Tuple<Regex, IPowerProcessor>>();
        public PowerProcessor()
        {
            this.BuildRegexMatchers();
        }

        /// <summary>
        /// Given an array of lines this will 
        /// <\summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public PowerViewModel ConvertFromMarkdown(List<string> lines)
        {
            var currentSectionContent = new List<string>();
            IPowerProcessor currentProcessor = null;
            var power = new PowerViewModel();

            foreach (var l in lines)
            {
                if (l == "___")
                {
                    // seperator line! skipping!
                    continue;
                }
                if (currentProcessor == null ||currentProcessor.Section == PowerSection.Unknown) // we don't know where we are, let's get our bearings
                {
                    currentProcessor = this.DetermineSectionByLine(l, currentProcessor);
                    currentSectionContent.Add(l);
                }
                else // we are in a section. Let's check this line and see if we happen to be at the end of a section
                {
                    if (currentProcessor.Section == PowerSection.Description && string.IsNullOrEmpty(l))
                    {
                        // Descriptions contain empty lines and we want to keep that. Just add the line and move on.
                        currentSectionContent.Add(l);
                        continue;
                    }
                    var procToUse = this.DetermineSectionByLine(l, currentProcessor);
                    if (procToUse == null)
                    {
                        // we hit a seperation line. Nothing to do on this line so please skip it
                        continue;
                    }
                    if (procToUse.Section == currentProcessor.Section) // we are still in the section we started in!
                    {
                        currentSectionContent.Add(l);
                    }
                    else
                    {
                        // the section we were in on th previous line is done. Let's process it!
                        power = currentProcessor.Process(currentSectionContent, power);
                        // then set reset the current setction and content and set the stuff up
                         currentProcessor = procToUse;
                        currentSectionContent = new List<string>{ l };
                    }
                }
            }

            if (currentSectionContent.Count > 0)
            {
                // there is something left in the buffer. Let's process it and move on!
                currentProcessor.Process(currentSectionContent, power);
            }
            return power;
        }

        private IPowerProcessor DetermineSectionByLine(string line, IPowerProcessor proc)
        {
            if (proc != null && proc.Section == PowerSection.Description &&
                (line.StartsWith("*") || line.StartsWith('#')))
            {
                if (this.SectionGuide[7].Item1.IsMatch(line))
                {
                    return this.SectionGuide[7].Item2; // the title processor
                }

                if (this.SectionGuide[5].Item1.IsMatch(line))
                {
                    return this.SectionGuide[5].Item2; // force potency processor
                }
                if (this.SectionGuide[6].Item1.IsMatch(line))
                {
                    return this.SectionGuide[6].Item2; // overcharge processor
                }

                return new DescriptionProcessor(); // if you are in description and one of these things didn't pop then you'ar estill in the description!
            }

            foreach (var section in this.SectionGuide)
            {
                var regex = section.Item1;
                if (regex.IsMatch(line))
                {
                    ;
                    return section.Item2;
                }
            }

            return null;
        }

        private void BuildRegexMatchers()
        {
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"####"), new TitleProcessor()));
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"^_[^_]{3,}_"), new LevelProcessor()));
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"-\s\*\*Prerequisite:\*\*"), new PrerequisiteProcessor()));
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"-\s\*\*Range:\*\* "), new RangeProcessor()));
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"-\s\*\*Duration"), new DurationProcessor()));
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"\*\*\*Force Potency"), new PotencyProcessor()));
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"\*\*\*Overcharge Tech"), new PotencyProcessor()));
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"-\s\*\*Casting Time:\*\*"), new CastingTimeProcessor()));
            this.SectionGuide.Add(new Tuple<Regex, IPowerProcessor>(new Regex(@"^\w"), new DescriptionProcessor()));
        }
    }

    public enum PowerSection
    {
        Unknown,
        Title,
        Level,
        CastingTime,
        Range,
        Duration,
        Description,
        Potency,
        Prerequisite,
    }
}