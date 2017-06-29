using Microsoft.AspNetCore.Mvc;
using System;
using WebStore.Data;
using SkiaSharp;
using System.IO;

namespace WebStore.Controllers
{
    public class ImagesController : Controller
    {
        private StoreContext StoreContext { get; }

        public ImagesController(StoreContext storeContext)
        {
            StoreContext = storeContext ?? throw new ArgumentNullException(nameof(storeContext));
        }

        public IActionResult Index(int id)
        {
            var image = StoreContext.Images.Find(id);

            if (image == null)
            {
                return NotFound();
            }

            return File(image.Content, image.MediaType, image.FileName);
        }

        public IActionResult Thumbnail(int id)
        {
            var image = StoreContext.Images.Find(id);

            if (image == null)
            {
                return NotFound();
            }

            var outputStream = new MemoryStream();

            using (var inputStream = new MemoryStream(image.Content))
            {
                ResizeImage(inputStream, outputStream, 150);
            }

            outputStream.Position = 0;

            return File(outputStream, image.MediaType, image.FileName);
        }

        public void ResizeImage(Stream input, Stream output, int size, int quality = 75)
        {
            using (var inputStream = new SKManagedStream(input))
            using (var original = SKBitmap.Decode(inputStream))
            {
                int width, height;

                if (original.Width > original.Height)
                {
                    width = size;
                    height = original.Height * size / original.Width;
                }
                else
                {
                    width = original.Width * size / original.Height;
                    height = size;
                }

                using (var resized = original.Resize(new SKImageInfo(width, height), SKBitmapResizeMethod.Lanczos3))
                {
                    if (resized == null)
                    {
                        return;
                    }

                    using (var image = SKImage.FromBitmap(resized))
                    {
                        image.Encode(SKEncodedImageFormat.Jpeg, quality)
                            .SaveTo(output);
                    }
                }
            }
            return;
        }
    }
}