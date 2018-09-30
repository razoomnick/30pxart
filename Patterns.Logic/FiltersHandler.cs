using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Patterns.Effects;
using Patterns.Svg;

namespace Patterns.Logic
{
    internal static class FiltersHandler
    {
        private static readonly Dictionary<String, IImageFilter> filters = new Dictionary<String, IImageFilter>
            {
                {"smoothing", new SmoothingFilter()},
                {"tiles", new TilesFilter()},
                {"embroidery", new EmbroideryFilter()},
                {"knitting", new KnittingFilter()},
                {"none", new ResizeFilter()}
            };

        private static readonly Dictionary<String, ISvgFilter> svgFilters = new Dictionary<string, ISvgFilter>
        {
            {"knitting", new SvgKnittingFilter()},
            {"embroidery", new SvgEmbroideryFilter()}
        }; 

        public static Bitmap ApplyFilter(Bitmap bitmap, int scale, string filterName)
        {
            var filter = filters.ContainsKey(filterName) ? filters[filterName] : null;
            var result = filter != null
                             ? filter.Apply(bitmap, scale)
                             : bitmap;
            return result;
        }

        public static Stream ApplySvgFilter(Bitmap bitmap, String filterName)
        {
            var filter = svgFilters[filterName];
            var result = filter != null
                             ? filter.ConvertToSvg(bitmap)
                             : null;
            return result;
        }
    }
}
