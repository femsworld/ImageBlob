// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using System.IO;
// using System.Threading.Tasks;

// namespace ImageBlob.Pages
// {
//     public class UploadPageModel : PageModel
//     {
//         private readonly IWebHostEnvironment _environment;

//         public UploadPageModel(IWebHostEnvironment environment)
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageBlob.Pages
{
    public class UploadPageModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public UploadPageModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            ImageDataList = new List<byte[]>();
        }

        [BindProperty]
        public IFormFile ImageFile { get; set; }
        public List<byte[]> ImageDataList { get; set; } // Store image data in a list

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(stream);
                    var imageData = stream.ToArray();
                    ImageDataList.Add(imageData); // Add image data to the list
                }
            }

            return Page();
        }
    }
}
