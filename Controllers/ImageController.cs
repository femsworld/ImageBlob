using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;

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
        public IActionResult GetBlobImage(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagecontainer");
            var blobClient = containerClient.GetBlobClient(blobName);

            if (blobClient.Exists())
            {
                var response = blobClient.OpenRead();
                var contentType = GetContentType(blobName);
                // var contentType = "image/jpeg";

                return File(response, contentType);
            }
            else
            {
                return NotFound();
            }
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
