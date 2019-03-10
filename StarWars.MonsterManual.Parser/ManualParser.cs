using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarWars.MonsterManual.Parser.SectionProcessors;
using StarWars.Storage;
using StarWars5e.Models.Monster;

namespace StarWars.MonsterManual.Parser
{
    /// <summary>
    /// This works through parsing an entire manual
    /// </summary>
    public class ManualParser
    {
        private readonly IMonsterRepository _monsterRepo;
        private string fileLocation;
        private List<Monster> monsters = new List<Monster>();

        public ManualParser(IMonsterRepository monsterRepo)
        {
            _monsterRepo = monsterRepo;
        }

        /// <summary>
        /// Convert a markdown file into a list of bakcgrounds that can be programmatically interacted with
        /// </summary>
        /// <param name="location">Location of the file</param>
        /// <param name="isRemote">Does an http call need to be made to get this markdown file</param>
        /// <param name="isInternal">Is this file included as an intenral resource in our assembly</param>
        /// <returns></returns>
        public async Task<List<Monster>> Process(string location, bool isRemote = false, bool isInternal = false)
        {
            var monsters = new List<Monster>();
            List<string> lines = new List<string>();

            if (isRemote)
            {
                lines = await this.ReadRemoteFile(location);
            }
            else if (isInternal)
            {
                lines = await this.ReadInternalFile(location);
            }

            var blocks = this.FindMonsters(lines);
            var proc = new MonsterProcessor("");
            foreach (var markdown in blocks)
            {
                monsters.Add(proc.ProcessLineList(markdown));
            }
            return monsters;
        }

        private List<List<string>> FindMonsters(List<string> lines)
        {
            var monsterList = new List<List<string>>();
            var current = new List<string>();
            var previousLine = "";
            var bgStart = "> ## ";

            foreach (var line in lines)
            {
                if (line.StartsWith(bgStart) && current.Count > 0)
                {
                    monsterList.Add(current);
                    current = new List<string>();
                }

                current.Add(line);
            }

            if (current.Count > 0)
            {
                monsterList.Add(current);
            }
            return monsterList;
        }

        private async Task<List<string>> ReadInternalFile(string location)
        {
            try
            {
                var lines = new List<string>();
                var ass = Assembly.GetEntryAssembly();
                var res = ass.GetManifestResourceNames();
                using (Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream($"StarWars.MonsterManual.Parser.Sources.{location}.md"))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 128))
                    {
                        string currentLine;
                        while ((currentLine = await reader.ReadLineAsync()) != null)
                        {
                            lines.Add(currentLine);
                            currentLine = null;
                        }
                    }
                }

                return lines;
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
        private async Task<List<string>> ReadRemoteFile(string location)
        {
            throw new NotImplementedException();
        }

        public async Task ReadFile(string location)
        {
            this.fileLocation = location;
            const int BufferSize = 128;
            using (var fileStream = File.OpenRead(this.fileLocation))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string line;
                string previousLine = null;
                var currentSectionText = new StringBuilder();
                var sectionType = TextSection.UnknownOrNone;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (sectionType == TextSection.UnknownOrNone)
                    {
                        sectionType = this.DetermineCurrentSection(line, previousLine);
                    }
                    else
                    {
                        var closeSection = this.CheckForSectionEnd(line, previousLine);
                        if (closeSection)
                        {
                            this.ProcessSection(sectionType, currentSectionText.ToString());
                            currentSectionText = new StringBuilder();
                            sectionType = TextSection.UnknownOrNone;
                        }
                    }

                    previousLine = line;
                    if (!string.IsNullOrEmpty(line) && sectionType != TextSection.UnknownOrNone) // maybe i want this???
                    {
                        currentSectionText.AppendLine(line);

                    }
                }

                Console.WriteLine($"There were {this.monsters.Count} monsters");
                await this._monsterRepo.InsertMonsters(this.monsters);
                Console.WriteLine("Done!");
                ;
            }
        }

        private void ProcessSection(TextSection sectionType, string contentValue)
        {
            switch (sectionType)
            {
                case TextSection.UnknownOrNone:
                    break;
                case TextSection.ChapterIntro:
                    break;
                case TextSection.SectionHeader:
                    break;
                case TextSection.Monster:
                    var monster = new MonsterProcessor(contentValue).Process();
                    this.monsters.Add(monster);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
            }
        }

        private bool CheckForSectionEnd(string input, string previousLine)
        {
            if (input == "___" && previousLine == "___")
            {
                return true;
            }

            if (input == "___" && string.IsNullOrEmpty(previousLine))
            {
                return true;
            }
            if (SectionEnders.ColumnBreak.IsMatch(input))
            {
                return true;
            }
            if (SectionEnders.PageBreak.IsMatch(input))
            {
                return true;
            }

            return false;
        }

        private TextSection DetermineCurrentSection(string input, string previousLine)
        {
            if (SectionBeginners.IsNewMonster(input, previousLine))
            {
                return TextSection.Monster;
            }
            if (SectionBeginners.ChapterInfo.IsMatch(input))
            {
                return TextSection.ChapterIntro;
            }

            if (SectionBeginners.SectionHeader.IsMatch(input))
            {
                return TextSection.SectionHeader;
            }

            return TextSection.UnknownOrNone;
        }
    }

    public class SectionEnders
    {
        public static readonly Regex ColumnBreak = new Regex(@"columnbreak");
        public static readonly Regex PageBreak = new Regex(@"pagebreakNum");

        public static Regex Footnote = new Regex(@"<div class='footnote'>");
    }

    public class SectionBeginners
    {
        public static readonly Regex ChapterInfo = new Regex(@"#?\s");
        public static readonly Regex SectionHeader = new Regex(@"##\s");

        public static bool IsNewMonster(string currentLine, string previousLine)
        {
            var condition1 = (string.IsNullOrEmpty(previousLine) || previousLine == "___") && currentLine == "___";
            var condition2 = previousLine == "___" && SectionHeader.IsMatch(currentLine);
            return condition1 || condition2;
        }
    }

    public enum TextSection
    {
        UnknownOrNone,
        ChapterIntro,
        SectionHeader,
        Monster
    }
}
