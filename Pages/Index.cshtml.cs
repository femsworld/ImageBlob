﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace ImageBlob.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
         private string _connectionString;

        public IndexModel(
            IWebHostEnvironment environment,
            IConfiguration configuration,
            IOptions<BlobStorageOptions> blobStorageOptions)
        {
            _environment = environment;
            _configuration = configuration;

            // Initialize BlobServiceClient using your connection string
            _blobServiceClient = new BlobServiceClient(blobStorageOptions.Value.ConnectionString);
        }

        [BindProperty]
        public IFormFile ImageFile { get; set; }
        public List<BlobItem> BlobItems { get; set; }
        // public List<byte[]> ImageDataList { get; set; }
        public List<string> ImageDataList { get; set; }
        public List<string> BlobImageUrls { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync()
        {
            BlobItems = new List<BlobItem>();

            // List all blobs in the container
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagecontainer");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                BlobItems.Add(blobItem);
                BlobImageUrls.Add(containerClient.GetBlobClient(blobItem.Name).Uri.ToString());
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(stream);

                    // Reset the stream position to the start of the data
                    stream.Position = 0;
                    var fileName = Path.GetFileName(ImageFile.FileName);

                    // Get a reference to a blob
                    var containerClient = _blobServiceClient.GetBlobContainerClient("imagecontainer");
                    var blobClient = containerClient.GetBlobClient(fileName); 

                    // Upload the image to Blob Storage
                    await blobClient.UploadAsync(stream, true);
                }
            }

            return RedirectToPage();
        }

    }
}


