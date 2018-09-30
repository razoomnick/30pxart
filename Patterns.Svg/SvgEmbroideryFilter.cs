using System;
using System.Drawing;
using Svg;

namespace Patterns.Svg
{
    public class SvgEmbroideryFilter: BaseSvgFilter
    {
        protected override void AddElementDefinition(SvgGroup group)
        {
            var pathCross = new SvgPath();
            var strPathCross = "M 1.65 0.65 a1.4 1.4 0 0 1 2 0 l 2.5 2.5 l 2.5 -2.5 a1.4 1.4 0 0 1 2 0 l 1 1 a1.4 1.4 0 0 1 0 2 l -2.5 2.5 l 2.5 2.5 a1.4 1.4 0 0 1 0 2 l -1 1 a1.4 1.4 0 0 1 -2 0 l -2.5 -2.5 l -2.5 2.5 a1.4 1.4 0 0 1 -2 0 l -1 -1 a1.4 1.4 0 0 1 0 -2 l 2.5 -2.5 l -2.5 -2.5 a1.4 1.4 0 0 1 0 -2 l 1 -1 z";
            pathCross.PathData = SvgPathBuilder.Parse(strPathCross);
            group.Children.Add(pathCross);
            var pathShadow = new SvgPath();
            var strPathShadow = "M11.7 2 a1.4 1.4 0 0 1 0 2 l-8 8 a1.4 1.4 0 0 1 -2 0 l10 -10 z";
            pathShadow.PathData = SvgPathBuilder.Parse(strPathShadow);
            pathShadow.Fill = new SvgColourServer(Color.Black);
            pathShadow.FillOpacity = 0.2f;
            group.Children.Add(pathShadow);
            var pathLight = new SvgPath();
            var strPathLight = "M8.7 0.65 a1.4 1.4 0 0 1 2 0 l-10 10 a1.4 1.4 0 0 1 0 -2 l10 -10 z";
            pathLight.PathData = SvgPathBuilder.Parse(strPathLight);
            pathLight.Fill = new SvgColourServer(Color.White);
            pathLight.FillOpacity = 0.2f;
            group.Children.Add(pathLight);
        }

        protected override bool UseBackgroundForMajorColor
        {
            get { return true; }
        }

        protected override SvgElement GetBackground(SvgDocument document, Bitmap bitmap)
        {
            var group = new SvgGroup(){ID = "background"};
            var background = new SvgRectangle()
            {
                Width = document.Width,
                Height = document.Height,
                Fill = new SvgColourServer(GetMajorColor(bitmap))
            };
            group.Children.Add(background);
            AddHatching(document, group, Color.Black, 0.05f, 0, false);
            AddHatching(document, group, Color.White, 0.05f, 2, false);
            AddHatching(document, group, Color.Black, 0.05f, 0, true);
            AddHatching(document, group, Color.White, 0.05f, 2, true);
            return group;
        }

        private static void AddHatching(SvgDocument document, SvgGroup group, Color color, float opacity, int margin, Boolean backDirection)
        {
            var w = document.Width;
            var h = document.Height;
            for (int i = margin; i < w + h; i += 4)
            {
                var x0 = i < h ? 0 : i - h;
                var y0 = i - x0;
                var x1 = (i + 1) < h ? 0 : (i + 1) - h;
                var y1 = (i + 1) - x1;
                var y2 = (i + 1) < w ? 0 : (i + 1) - w;
                var x2 = (i + 1) - y2;
                var y3 = i < w ? 0 : i - w;
                var x3 = i - y3;
                if (backDirection)
                {
                    x0 = w - x0;
                    x1 = w - x1;
                    x2 = w - x2;
                    x3 = w - x3;
                }
                var strPathTemplate = "M{0} {1} L{2} {3} L{4} {5} L{6} {7} z";
                var strPath = String.Format(strPathTemplate, x0, y0, x1, y1, x2, y2, x3, y3);
                var path = new SvgPath();
                path.PathData = SvgPathBuilder.Parse(strPath);
                path.Fill = new SvgColourServer(color);
                path.FillOpacity = opacity;
                group.Children.Add(path);
            }
        }
    }
}
