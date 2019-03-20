﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.ViewModels;
using StarWars5e.Species.Parser.Processors;
using StarWars5e.Storage.Repositories;

namespace StarWars5e.Species.Parser
{
    public class SpeciesParser
    {
        private readonly ISpeciesRepository _repo;

        public SpeciesParser(ISpeciesRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Convert a markdown file into a list of species that can be programmatically interacted with
        /// </summary>
        /// <param name="location">Location of the file</param>
        /// <param name="isRemote">Does an http call need to be made to gte this mardown file</param>
        /// <param name="isInternal">Is this file included as an intenral resource in our assembly</param>
        /// <returns></returns>
        public async Task<List<SpeciesViewModel>> Process(string location, bool isRemote = false, bool isInternal = false)
        {
            var speciesList = new List<SpeciesViewModel>();
            var coreProcessor = new CoreSpeciesProcessor();
            List<string> lines = new List<string>();

            if (isRemote)
            {
                lines = await this.ReadRemoteFile(location);
            }
            else if (isInternal)
            {
                lines = await this.ReadInternalFile(location);
            }

            var blocks = this.FindSpeciesBlocks(lines);
            foreach (var species in blocks)
            {
                speciesList.Add(coreProcessor.CreateSpecies(species));
            }
            return speciesList;
        }

        private List<List<string>> FindSpeciesBlocks(List<string> lines)
        {
            var species = new List<List<string>>();
            var current = new List<string>();
            var previousLine = "";
            var startRegex = new Regex(@">\s##\s");

            foreach (var line in lines)
            {
                if (previousLine == "___" && startRegex.IsMatch(line)) // this represents the beginning of a new block!
                {
                    if (current.Count > 1)
                    {
                        species.Add(current);
                        current = new List<string>();
                    }

                }

                current.Add(line);
                previousLine = line;
            }

            if (current.Count > 0)
            {
                species.Add(current);
            }
            return species;
        }

        private async Task<List<string>> ReadInternalFile(string location)
        {
            try
            {
                var lines = new List<string>();
                var ass = Assembly.GetEntryAssembly();
                var res = ass.GetManifestResourceNames();
                using (Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream($"StarWars.Species.Parser.Sources.{location}.md"))
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