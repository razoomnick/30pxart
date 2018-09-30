using System;
using System.Drawing;

namespace Patterns.Effects
{
    public class SmoothingFilter : IImageFilter
    {
        public Bitmap Apply(Bitmap bitmap, int scale)
        {
            if (scale == 1)
            {
                bitmap = hqx.HqxSharp.Scale2(bitmap, 48, 7, 6, 0, true, true);
                bitmap = ImagesHelper.ResizeImage(bitmap, 0.5f);
            }
            if (scale == 2)
            {
                bitmap = hqx.HqxSharp.Scale2(bitmap, 48, 7, 6, 0, true, true);
            }
            else if (scale == 3)
            {
                bitmap = hqx.HqxSharp.Scale3(bitmap, 48, 7, 6, 0, true, true);
            }
            else if (scale == 4)
            {
                bitmap = hqx.HqxSharp.Scale4(bitmap, 48, 7, 6, 0, true, true);
            }
            else if (scale > 4)
            {
                throw new ArgumentException("Smoothing can be used with scale from 1 to 4");
            }
            return bitmap;
        }
    }
}
