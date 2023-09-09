using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public List<byte[]> ImageDataList { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(stream);
                    var imageData = stream.ToArray();
                    ImageDataList.Add(imageData);
                }
            }

            return Page();
        }
    }
}
