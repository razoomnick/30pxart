using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Patterns.Cloud.Gcs;
using Patterns.Data.Repositories;
using Patterns.Effects;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;
using Patterns.Svg;

namespace Patterns.Logic
{
    public class ImagesLogic
    {
        private readonly IPatternImagesRepository patternImagesRepository = new PatternImagesRepository();

        public PatternImage GetImage(Guid id)
        {
            var image = patternImagesRepository.GetById(id);
            return image;
        }

        public void SaveImage(PatternImage patternImage)
        {
            var uploader = new GcsUploader(HttpContext.Current.Request.MapPath("~"));
            var extension = "." + patternImage.ContentType.Replace("image/", "");
            var name = patternImage.Id.ToString() + extension;
            patternImage.CdnUrl = uploader.Upload(name, patternImage.ContentType, patternImage.RawData);
            patternImagesRepository.Save(patternImage);
        }

        public PatternImage CanvasesToPatternImage(ApiPattern pattern, int scale, String filterName)
        {
            if (scale < 1 || scale > 4)
            {
                throw new ArgumentException("Scale can't be less than 1 and more than 4");
            }

            var mergedBitmap = GetMergedBitmap(pattern);
            mergedBitmap = FiltersHandler.ApplyFilter(mergedBitmap, scale, filterName);

            byte[] bytes;
            String contentType;
            using (var stream = new MemoryStream())
            {
                var pixelsCount = mergedBitmap.Width*mergedBitmap.Height;
                if (pixelsCount < 500*500)
                {
                    mergedBitmap.Save(stream, ImageFormat.Png);
                    contentType = "image/png";
                }
                else
                {
                    var quality = pixelsCount < 700*700
                                      ? 100L
                                      : pixelsCount < 1000*1000
                                            ? 80L
                                            : 60L;
                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    Encoder myEncoder = Encoder.Quality;
                    var myEncoderParameters = new EncoderParameters(1);
                    var myEncoderParameter = new EncoderParameter(myEncoder, quality);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    mergedBitmap.Save(stream, jgpEncoder, myEncoderParameters);
                    contentType = "image/jpeg";
                }
                stream.Close();
                bytes = stream.ToArray();
            }
            var result = new PatternImage()
                {
                    RawData = bytes,
                    ContentType = contentType
                };
            return result;
        }

        private static Bitmap GetMergedBitmap(ApiPattern pattern)
        {
            var canvases = pattern.Canvases;
            var visibilities = pattern.LayersVisibility;
            var img = Base64ToImage(pattern.Canvases[0]);
            var mergedBitmap = new Bitmap(img.Width, img.Height);

            using (var graphics = Graphics.FromImage(mergedBitmap))
            {
                graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, mergedBitmap.Width, mergedBitmap.Height);
                for (var i = canvases.Count - 1; i >= 0; i--)
                {
                    if (visibilities == null || visibilities.Count < i || visibilities[i])
                    {
                        var image = Base64ToImage(canvases[i]);
                        var bitmap = new Bitmap(image);
                        graphics.DrawImage(bitmap, 0, 0);
                    }
                }
            }
            return mergedBitmap;
        }

        private static Image Base64ToImage(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = Image.FromStream(ms, true);
            return image;
        }

        public static byte[] CreateAvatar(Stream fileStream)
        {
            var image = Image.FromStream(fileStream);
            var size = Math.Min(image.Width, image.Height);
            var xOffset = (image.Width - size)/2;
            var yOffset = (image.Height - size)/2;
            var area = new Rectangle(xOffset, yOffset, size, size);
            var square = CropImage(image, area);
            var scale = 60f/size;
            var avatar = ImagesHelper.ResizeImage((Bitmap)square, scale);
            byte[] result;
            using (var stream = new MemoryStream())
            {
                avatar.Save(stream, ImageFormat.Png);
                stream.Close();
                result = stream.ToArray();
            }
            return result;
        }

        public static byte[] RepeatImage(byte[] pngRawData, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);
            var tile = new Bitmap(new MemoryStream(pngRawData));
            
            for (int x = 0; x < width; x += tile.Width)
            {
                for (int y = 0; y < height; y += tile.Height)
                {
                    graphics.DrawImage(tile, x, y);
                }
            }
            byte[] result;
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Close();
                result = stream.ToArray();
            }
            return result;
        }

        private static Image CropImage(Image image, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(image);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        public static Stream GetVector(ApiPattern pattern, string filter)
        {
            var bitmap = GetMergedBitmap(pattern);
            var stream = FiltersHandler.ApplySvgFilter(bitmap, filter);
            return stream;
        }
    }
}
