using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Parsers
{
    public abstract class BaseProcessor<T>: IBaseProcessor<T> where T: class
    {
        public async Task<List<T>> Process(List<string> locations, Language language = Language.en)
        {
            var lines = await ReadInternalFile(locations, language);
            
            var blocks = await FindBlocks(lines);
            return blocks;
        }

        private static async Task<List<string>> ReadInternalFile(IEnumerable<string> locations, Language language)
        {
            try
            {
                var lines = new List<string>();

                foreach (var location in locations)
                {
                    using (var stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream($"StarWars5e.Parser.Sources.{language}.{location}"))
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
    }
}
