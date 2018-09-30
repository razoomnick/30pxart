using System.Drawing;
using System.Reflection;

namespace Patterns.Effects
{
    public class KnittingFilter: IImageFilter
    {
        private Bitmap embroidery;

        private void Initialize()
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var embroideryFile = thisAssembly.GetManifestResourceStream("Patterns.Effects.Resources.knitting8.png");
            embroidery = new Bitmap(embroideryFile);
        }

        public Bitmap Apply(Bitmap bitmap, int scale)
        {
            Initialize();
            var resizedImage = ImagesHelper.ResizeImage(bitmap, 8);
            PrepareKnitting(resizedImage);
            var repeatedEmbroidery = ImagesHelper.RepeatToFitSize(embroidery, resizedImage.Width, resizedImage.Height);
            var result = ImagesHelper.MergeBitmaps(repeatedEmbroidery, resizedImage);
            return result;
        }

        private void PrepareKnitting(Bitmap image)
        {
            for (var i = 0; i < image.Height; i += 8)
            {
                for (var j = 2; j < image.Width + 3; j += 8)
                {
                    for (var dx = 0; dx < 4; dx++)
                    {
                        var ySource = (i + image.Height - 1)%image.Height;
                        var x = (j + dx) % image.Width;
                        image.SetPixel(x, i, image.GetPixel(x, ySource));
                    }
                    for (var dx = 0; dx < 2; dx++)
                    {
                        var ySource = (i + image.Height - 1)%image.Height;
                        var x = (j + 1 + dx) % image.Width;
                        var y = (i + 1) % image.Height;
                        image.SetPixel(x, y, image.GetPixel(x, ySource));
                    }
                }
            }
        }
    }
}
