using System;
using System.Drawing;

namespace Patterns.Effects
{
    public interface IImageFilter
    {
        Bitmap Apply(Bitmap bitmap, int scale);
    }
}