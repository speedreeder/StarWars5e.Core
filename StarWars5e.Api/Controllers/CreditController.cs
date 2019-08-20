using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;

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
        public async Task<ActionResult<string>> Get()
        {
            var container = _cloudBlobClient.GetContainerReference("credits");
            var blockBlob = container.GetBlockBlobReference("credits.txt");

            var stream = new MemoryStream();
            await blockBlob.DownloadToStreamAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);
            var x = new StreamReader(stream).ReadToEnd();

            return Ok(x);
        }
    }
}
