using System.Drawing;
using System.Reflection;

namespace Patterns.Effects
{
    public class EmbroideryFilter: IImageFilter
    {
        private Bitmap embroidery;

        private void Initialize()
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var embroideryFile = thisAssembly.GetManifestResourceStream("Patterns.Effects.Resources.embroidery8.png");
            embroidery = new Bitmap(embroideryFile);
        }

        public Bitmap Apply(Bitmap bitmap, int scale)
        {
            Initialize();
            var resizedImage = ImagesHelper.ResizeImage(bitmap, 8);
            var repeatedEmbroidery = ImagesHelper.RepeatToFitSize(embroidery, resizedImage.Width, resizedImage.Height);
            var result = ImagesHelper.MergeBitmaps(repeatedEmbroidery, resizedImage);
            return result;
        }
    }
}
