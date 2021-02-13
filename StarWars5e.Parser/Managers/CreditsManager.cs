using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Localization;

namespace StarWars5e.Parser.Managers
{
    public class CreditsManager
    {
        private readonly ILocalization _localization;
        private readonly BlobContainerClient _blobContainerClient;

        public CreditsManager(IServiceProvider serviceProvider, ILocalization localization)
        {
            _localization = localization;
            var blobServiceClient = serviceProvider.GetService<BlobServiceClient>();
            _blobContainerClient = blobServiceClient.GetBlobContainerClient($"credits-{_localization.Language}");
        }

        public async Task Parse()
        {
            try
            {
                var lines = new List<string>();

                using (var stream = Assembly.GetEntryAssembly()
                    ?.GetManifestResourceStream($"StarWars5e.Parser.Sources.{_localization.Language}.Credits.txt"))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream, Encoding.UTF8, true, 128))
                    {
                        string currentLine;
                        while ((currentLine = await reader.ReadLineAsync()) != null)
                        {
                            lines.Add(currentLine);
                        }
                    }

                    lines = lines.CleanListOfStrings().ToList();

                    await _blobContainerClient.CreateIfNotExistsAsync();

                    var blobClient = _blobContainerClient.GetBlobClient("credits.txt");

                    var content = Encoding.UTF8.GetBytes(string.Join("\r\n", lines));
                    using (var ms = new MemoryStream(content))
                    {
                        await blobClient.UploadAsync(ms, true);
                    }
                }

                
            }
            catch (Exception)
            {
                Console.WriteLine("Failed while uploading the credits.");
            }
        }
    }
}
