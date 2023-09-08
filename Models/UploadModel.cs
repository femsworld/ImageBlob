namespace ImageBlob.Models
{
    public class UploadModel
    {
        public IFormFile ImageFile { get; set; }
        public byte[] ImageData { get; set; }
    }
}