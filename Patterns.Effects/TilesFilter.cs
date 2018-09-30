using System.Drawing;
using System.Reflection;

namespace Patterns.Effects
{
    public class TilesFilter: IImageFilter
    {
        private Bitmap tiles;
        private Bitmap mask;

        private void Initialize()
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var maskFile = thisAssembly.GetManifestResourceStream("Patterns.Effects.Resources.tiles8.png");
            tiles = new Bitmap(maskFile);
            var invertedMaskFile = thisAssembly.GetManifestResourceStream("Patterns.Effects.Resources.tiles8inverted.png");
            mask = new Bitmap(invertedMaskFile);
        }

        public Bitmap Apply(Bitmap bitmap, int scale)
        {
            Initialize();
            var resizedImage = ImagesHelper.ResizeImage(bitmap, 8);
            var darkenImage = ImagesHelper.Adjust(resizedImage, 0.4f, 1, 1);
            var repeatedTiles = ImagesHelper.RepeatToFitSize(tiles, resizedImage.Width, resizedImage.Height);
            var repeatedMask = ImagesHelper.RepeatToFitSize(mask, resizedImage.Width, resizedImage.Height);
            var darkenLayer = ImagesHelper.ApplyAlphaMask(darkenImage, repeatedMask);
            var result = ImagesHelper.MergeBitmaps(darkenLayer, repeatedTiles, resizedImage);
            return result;
        }
    }
}
