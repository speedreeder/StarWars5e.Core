using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Localization;

namespace StarWars5e.Parser.Managers
{
    public class CreditsManager
    {
        private readonly CloudBlobContainer _cloudBlobContainer;
        private readonly ILocalization _localization;

        public CreditsManager(CloudStorageAccount cloudStorageAccount, ILocalization localization)
        {
            _localization = localization;
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference("credits");
        }

        public async Task Parse()
        {
            try
            {
                var lines = new List<string>();

                await _cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Off, null, null);

                var blob = _cloudBlobContainer.GetBlockBlobReference($"credits.{_localization}.txt");
                using (var stream = Assembly.GetEntryAssembly()
                    .GetManifestResourceStream($"StarWars5e.Parser.Sources.{_localization}.Credits.txt"))
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
                    await blob.UploadTextAsync(string.Join("\r\n", lines));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed while uploading the credits.");
            }
        }
    }
}
