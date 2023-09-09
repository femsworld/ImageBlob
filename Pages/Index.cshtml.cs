// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;

// namespace ImageBlob.Pages
// {
//     public class IndexModel : PageModel
//     {
//         private readonly IWebHostEnvironment _environment;

//         public IndexModel(IWebHostEnvironment environment)
//         {
//             _environment = environment;
//         }

//         [BindProperty]
//         public IFormFile ImageFile { get; set; }
//         public byte[] ImageData { get; set; }

//         public async Task<IActionResult> OnPostAsync()
//         {
//             if (ImageFile != null && ImageFile.Length > 0)
//             {
//                 using (var stream = new MemoryStream())
//                 {
//                     await ImageFile.CopyToAsync(stream);
//                     ImageData = stream.ToArray();
//                 }
//             }

//             return Page();
//         }
//     }
// }


using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ImageBlob.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;

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
        public List<byte[]> ImageDataList { get; set; } // Add this property

        public async Task<IActionResult> OnGetAsync()
        {
            BlobItems = new List<BlobItem>();
            ImageDataList = new List<byte[]>(); // Initialize the ImageDataList

            // List all blobs in the container
            var containerClient = _blobServiceClient.GetBlobContainerClient("imagecontainer");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                BlobItems.Add(blobItem);
            }

            return Page();
        }

        // public async Task<IActionResult> OnPostAsync()
        // {
        //     if (ImageFile != null && ImageFile.Length > 0)
        //     {
        //         using (var stream = new MemoryStream())
        //         {
        //             await ImageFile.CopyToAsync(stream);

        //             // Get a reference to a blob
        //             var containerClient = _blobServiceClient.GetBlobContainerClient("imagecontainer");
        //             var fileName = "quickstart" + Guid.NewGuid().ToString() + ".png";
        //             var blobClient = containerClient.GetBlobClient(fileName);

        //             // Upload the image to Blob Storage
        //             await blobClient.UploadAsync(stream, true);
        //         }
        //     }

        //     return RedirectToPage();
        // }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(stream);

                    // Reset the stream position to the start of the data
                    stream.Position = 0;

                    // Get a reference to a blob
                    var containerClient = _blobServiceClient.GetBlobContainerClient("testcontainer");
                    var fileName = "quickstart" + Guid.NewGuid().ToString() + ".png";
                    var blobClient = containerClient.GetBlobClient(fileName);

                    // Upload the image to Blob Storage
                    await blobClient.UploadAsync(stream, true);
                }
            }

            return RedirectToPage();
        }

    }
}


