using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models.ViewModels;
using StarWars5e.Powers.Parser.Processors;
using StarWars5e.Storage.Repositories;

namespace StarWars5e.Powers.Parser
{

    /// <summary>
    /// Orchestrator to parse through a (marginally) formatted bit of markdown that represents force or tech powers
    /// </summary>
    public class PowerParser
    {
        public readonly string PowerTitle = "#### ";
        private readonly IPowerRepository _powerRepo;
        private PowerProcessor proc;

        public PowerParser(IPowerRepository powerRepo)
        {
            _powerRepo = powerRepo;
            this.proc = new PowerProcessor();
        }

        /// <summary>
        /// Convert a markdown file into a list of powers that can be programmatically interacted with
        /// </summary>
        /// <param name="location">Location of the file</param>
        /// <param name="powerType">Type of power. Must be 'force' or 'tech'</param>
        /// <param name="isRemote">Does an http call need to be made to gte this mardown file</param>
        /// <param name="isInternal">Is this file included as an intenral resource in our assembly</param>
        /// <returns></returns>
        public async Task<List<PowerViewModel>> Process(string location, string powerType, bool isRemote = false, bool isInternal = true)
        {
            if (powerType != "force" && powerType != "tech")
            {
                throw new ArgumentException("You must be converting either for or tech powers");
            }
            List<string> lines = new List<string>();

            if (isRemote)
            {
                lines = await this.ReadRemoteFile(location);
            }

            if (isInternal)
            {
                lines = await this.ReadInternalFile(location);
            }

            var powers = this.BreakdownPowers(lines);
            return this.ProcessPowerList(powers, powerType);
            
        }

        private List<PowerViewModel> ProcessPowerList(List<List<string>> powers, string powerType)
        {
            var converted = new List<PowerViewModel>();
            foreach (var power in powers)
            {
                var vm = this.proc.ConvertFromMarkdown(power);
                vm.PowerType = powerType;
                converted.Add(vm);
            }

            return converted;
        }

        /// <summary>
        /// Parse through the entire set of lines and figure out wherein the powers lie :)
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="powerType"></param>
        /// <returns></returns>
        private List<List<string>> BreakdownPowers(List<string> lines)
        {
            var powers = new List<List<string>>();
            var currentPower = new List<string>();

            foreach (var line in lines)
            {
                // if we have a title and there is a power in progress then let's move onto the new one
                if (line.StartsWith(this.PowerTitle) && currentPower.Count > 0)
                {
                    powers.Add(currentPower);
                    currentPower = new List<string>();
                    currentPower.Add(line);
                    continue;
                }
                currentPower.Add(line);

            }

            // if we reached the end of the list let's make sure we add whatever is in the current section to that array
            if (currentPower.Count > 0)
            {
                powers.Add(currentPower);
                currentPower = new List<string>();
            }
            return powers;
        }

        private async Task<List<string>> ReadInternalFile(string location)
        {
            try
            {
                var lines = new List<string>();
                var ass = Assembly.GetEntryAssembly();
                var res = ass.GetManifestResourceNames();
                using (Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream($"StarWars.Powers.Parser.Sources.{location}.md"))
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