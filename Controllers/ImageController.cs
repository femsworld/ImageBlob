using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageBlob.Controllers
{
    public class ImageController : Controller
    {
        private readonly BlobServiceClient _blobServiceClient;

        public ImageController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlobImage(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("quickstartblobs");
            var blobClient = containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.OpenReadAsync();

                // Determine the content type based on the file extension
                var contentType = GetContentType(blobName);

                // Return the file with the appropriate content type
                return File(response, contentType);
            }

            return NotFound();
        }

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                // Add more content types for other file extensions as needed
                _ => "application/octet-stream", // Default content type for other files
            };
        }
    }
}
