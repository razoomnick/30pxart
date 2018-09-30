using System.Drawing;
using System.IO;

namespace Patterns.Svg
{
    public interface ISvgFilter
    {
        Stream ConvertToSvg(Bitmap bitmap);
    }
}