using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;

namespace StarWars5e.Parser.Processors
{
    public abstract class BaseProcessor<T>: IBaseProcessor<T> where T: class
    {
        public ILocalization Localization;

        public List<string> FileNames;

        public async Task<List<T>> Process(List<string> locations, ILocalization localization)
        {
            Localization = localization;
            FileNames = locations;
            var lines = await ReadInternalFile(locations, localization);
            
            var blocks = await FindBlocks(lines);
            return blocks;
        }

        public async Task<List<T>> Process(List<string> locations, ILocalization localization, ContentType contentType)
        {
            Localization = localization;
            FileNames = locations;

            var lines = await ReadInternalFile(locations, localization);

            var blocks = await FindBlocks(lines, contentType);
            return blocks;
        }

        private static async Task<List<string>> ReadInternalFile(IEnumerable<string> locations, ILocalization localization)
        {
            try
            {
                var lines = new List<string>();

                foreach (var location in locations)
                {
                    using (var stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream($"StarWars5e.Parser.Sources.{localization.Language}.{location}"))
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

        public virtual Task<List<T>> FindBlocks(List<string> lines)
        {
            return null;
        }

        public virtual Task<List<T>> FindBlocks(List<string> lines, ContentType contentType)
        {
            return null;
        }
    }
}
