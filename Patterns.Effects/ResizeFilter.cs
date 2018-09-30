using System.Drawing;

namespace Patterns.Effects
{
    public class ResizeFilter: IImageFilter
    {
        public Bitmap Apply(Bitmap bitmap, int scale)
        {
            return ImagesHelper.ResizeImage(bitmap, scale);
        }
    }
}
