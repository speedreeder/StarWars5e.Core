using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CreditController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get(Language language = Language.en)
        {
            var blobContainerClient =
                new BlobContainerClient(_configuration["StorageAccountConnectionString"], $"credits-{language}");
            var exists = await blobContainerClient.ExistsAsync();

            if (!exists)
            {
                blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"],
                    $"credits-{Language.en}");
            }

            var blob = blobContainerClient.GetBlobClient("credits.txt");

            var stream = new MemoryStream();
            await blob.DownloadToAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);
            var creditsString = await new StreamReader(stream).ReadToEndAsync();

            return Ok(creditsString);
        }
    }
}
