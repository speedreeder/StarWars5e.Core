using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public CreditController(CloudBlobClient cloudBlobClient)
        {
            _cloudBlobClient = cloudBlobClient;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get(Language language = Language.en)
        {
            var container = _cloudBlobClient.GetContainerReference($"credits-{language}");
            var exists = await container.ExistsAsync();

            if (!exists)
            {
                container = _cloudBlobClient.GetContainerReference($"credits-{Language.en}");
            }

            var blockBlob = container.GetBlockBlobReference("credits.txt");

            var stream = new MemoryStream();
            await blockBlob.DownloadToStreamAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);
            var creditsString = new StreamReader(stream).ReadToEnd();

            return Ok(creditsString);
        }
    }
}
