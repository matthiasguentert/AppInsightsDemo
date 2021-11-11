using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApi.Controllers
{
    [Route("api/store")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly BlobContainerClient _client;

        public StorageController(BlobContainerClient client) => _client = client;

        // $"api/store/{Guid.NewGuid()}.txt";
        [HttpPut("{filename}")]
        public async Task<IActionResult> CreateRandomBlobAsync(string filename)
        {
            _client.CreateIfNotExists();

            var blobClient = _client.GetBlobClient(filename);

            var content = "Application Insights Demo";
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            var uri = blobClient.Uri;
            try
            {
                _ = await blobClient.UploadAsync(memoryStream);
            }
            catch (RequestFailedException e)
            {
                return Conflict(); // 409, already exists
            }

            return Created(uri, content); // Success 201
        }
    }
}
