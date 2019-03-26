using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Parsers
{
    public abstract class StarshipBaseProcessor<T>: IStarshipBaseProcessor<T> where T: class
    {
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
                using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream($"StarWars5e.Parser.Sources.{location}.md"))
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

        private async Task<List<string>> ReadRemoteFile(string location)
        {
            throw new NotImplementedException();
        }

    }
}
