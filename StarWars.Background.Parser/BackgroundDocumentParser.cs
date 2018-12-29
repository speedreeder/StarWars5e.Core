using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars.Background.Parser.Processors;
using StarWars5e.Models.ViewModels;

namespace StarWars.Background.Parser
{
    public class BackgroundDocumentParser
    {

        /// <summary>
        /// Convert a markdown file into a list of bakcgrounds that can be programmatically interacted with
        /// </summary>
        /// <param name="location">Location of the file</param>
        /// <param name="isRemote">Does an http call need to be made to get this markdown file</param>
        /// <param name="isInternal">Is this file included as an intenral resource in our assembly</param>
        /// <returns></returns>
        public async Task<List<BackgroundViewModel>> Process(string location, bool isRemote = false, bool isInternal = false)
        {
            var backgrounds = new List<BackgroundViewModel>();
            var coreProcessor = new CoreProcessor();
            List<string> lines = new List<string>();

            if (isRemote)
            {
                lines = await this.ReadRemoteFile(location);
            }
            else if (isInternal)
            {
                lines = await this.ReadInternalFile(location);
            }

            var blocks = this.FindBackgrounds(lines);
            foreach (var markdown in blocks)
            {
                backgrounds.Add(coreProcessor.ConvertMarkdownToBackground(markdown));
            }
            return backgrounds;
        }

        private List<List<string>> FindBackgrounds(List<string> lines)
        {
            var bgList = new List<List<string>>();
            var current = new List<string>();
            var previousLine = "";
            var bgStart = "## ";

            foreach (var line in lines)
            {
                if (line.StartsWith(bgStart) && current.Count > 0)
                {
                    bgList.Add(current);
                    current = new List<string>();
                }

                current.Add(line);
            }

            if (current.Count > 0)
            {
                bgList.Add(current);
            }
            return bgList;
        }


        private async Task<List<string>> ReadInternalFile(string location)
        {
            try
            {
                var lines = new List<string>();
                var ass = Assembly.GetEntryAssembly();
                var res = ass.GetManifestResourceNames();
                using (Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream($"StarWars.Background.Parser.Sources.{location}.md"))
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
    }
}