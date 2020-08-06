using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace PALM.ImageOperations
{

    public class ImageProcessor
    {
        //public static Stream ResizeImage(Stream imgStream, int width, int height)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imgStream.Seek(0, SeekOrigin.Begin);
        //    using (Image img = Image.FromStream(imgStream))
        //    {
        //        Bitmap bmp = new Bitmap(img, new Size(width, height));
        //        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //    }
        //    return ms;
        //}

        public static Stream ResizeImage(Stream imgStream, int width, int height)
        {
            MemoryStream ms = new MemoryStream();

            using (var outStream = new MemoryStream())
            using (var image = Image.Load(imgStream))
            {
                var clone = image.Clone(
                                i => i.Resize(width, height)
                                      .Crop(new Rectangle(0, 0, width, height)));

                clone.Save(ms, new  PngEncoder());
            }

            return ms;
        }
    }
}
