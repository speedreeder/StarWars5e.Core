using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Starships.Parser.Processors
{
    public abstract class StarshipBaseProcessor<T> where T: class
    {
        /// <summary>
        /// Convert a markdown file into a list of starship modifications that can be programmatically interacted with
        /// </summary>
        /// <param name="location">Location of the file</param>
        /// <param name="isRemote">Does an http call need to be made to get this mardown file?</param>
        /// <returns></returns>
        public async Task<List<T>> Process(string location, bool isRemote = false)
        {
            List<string> lines;

            if (isRemote)
            {
                lines = await ReadRemoteFile(location);
            }
            else
            {
                lines = await ReadInternalFile(location);
            }

            var blocks = await FindBlocks(lines);
            return blocks;
        }

        private static async Task<List<string>> ReadInternalFile(string location)
        {
            try
            {
                var lines = new List<string>();
                using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream($"StarWars.Starships.Parser.Sources.{location}.md"))
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8, true, 128))
                    {
                        string currentLine;
                        while ((currentLine = await reader.ReadLineAsync()) != null)
                        {
                            lines.Add(currentLine);
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

        public abstract Task<List<T>> FindBlocks(List<string> lines);

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
